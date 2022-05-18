using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Casino.Networking
{
    public class ServerRaw
    {
        string ip = "127.0.0.1";
        int port = 8080;

        TcpClient client;
        NetworkStream stream;

        const string eol = "\r\n";

        public ServerRaw()
        {
            var server = new TcpListener(IPAddress.Parse(ip), port);

            server.Start();
            Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection...", ip, port);

            client = server.AcceptTcpClient();

            Console.WriteLine("Client connected");

            stream = client.GetStream();
        }

        public void FetchAvailableData()
        {
            if (!stream.DataAvailable)
                return;

            while (client.Available < 3) ;

            Console.WriteLine("Got enough data");
            byte[] bytes = new byte[client.Available];

            stream.Read(bytes, 0, bytes.Length);

            string data = Encoding.UTF8.GetString(bytes);

            if (Regex.IsMatch(data, "^GET"))
            {
                Console.WriteLine("Get request received");
                Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                + "Connection: Upgrade" + eol
                + "Upgrade: websocket" + eol
                + "Sec-WebSocket-Accept: " + Convert.ToBase64String(System.Security.Cryptography.SHA1.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(
                            new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                        ))) + eol
                + eol);

                stream.Write(response, 0, response.Length);
                Console.WriteLine("Sent response");
            }
            else
            {
                bool fin = (bytes[0] & 0b10000000) != 0,
                    mask = (bytes[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"
                int opcode = bytes[0] & 0b00001111, // expecting 1 - text message
                    offset = 2;
                ulong msglen = (ulong)(bytes[1] & 0b01111111);

                if (msglen == 126)
                {
                    // bytes are reversed because websocket will print them in Big-Endian, whereas
                    // BitConverter will want them arranged in little-endian on windows
                    msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
                    offset = 4;
                }
                else if (msglen == 127)
                {
                    // To test the below code, we need to manually buffer larger messages — since the NIC's autobuffering 
                    // may be too latency-friendly for this code to run (that is, we may have only some of the bytes in this
                    // websocket frame available through client.Available).  
                    msglen = BitConverter.ToUInt64(new byte[] { (byte)data[9], (byte)data[8], (byte)data[7], (byte)data[6], (byte)data[5], (byte)data[4], (byte)data[3], (byte)data[2] });
                    offset = 10;
                }

                if (msglen == 0)
                    Console.WriteLine("msglen == 0");
                else if (mask)
                {
                    byte[] decoded = new byte[msglen];
                    byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
                    offset += 4;

                    for (ulong i = 0; i < msglen; ++i)
                        decoded[i] = (byte)(bytes[(ulong)offset + i] ^ masks[i % 4]);

                    string text = Encoding.UTF8.GetString(decoded);
                    Console.WriteLine("{0}", text);
                }
                else
                {
                    Console.WriteLine("mask bit not set");
                }

                // byte[] response = Encoding.UTF8.GetBytes("hiya");

                // stream.Write(response, 0, response.Length);
                Console.WriteLine();
            }
        }
    }
}