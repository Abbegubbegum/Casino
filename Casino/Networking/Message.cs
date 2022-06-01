namespace Casino.Networking
{
    public struct Message
    {
        public string sender { get; set; }

        public string type { get; set; }

        public string data { get; set; }
    }
}