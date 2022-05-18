using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Casino.Networking.WebsocketServices
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }
    }
}