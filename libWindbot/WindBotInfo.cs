using System;

namespace WindBot
{
    public class WindBotInfo
    {
        public string Name { get; set; }
        public string Deck { get; set; }
        public string Dialog { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string HostInfo { get; set; }
        public int Version { get; set; }
        public int Hand { get; set; }

        public WindBotInfo()
        {
            Name = "WindBotMobile";
            Deck = null;
            Dialog = "default";
            Host = "127.0.0.1";
            //Host = "192.168.1.233";
            Port = 7911;
            HostInfo = "";
            Version = 0x1343;
            Hand = 0;
        }
    }
}
