using System.Net.Sockets;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Quiz.Controllers
{
    public class TcpController
    {
        public Dictionary<int, int> Values { get; set; }
        
        public TcpClient Client { get; }

        public TcpController(short valueCount)
        {
            this.Values = Enumerable.Range(1, valueCount).ToDictionary(x => x, x => 0);
            this.Client = new TcpClient();
            this.Client.Connect("88.212.241.115", 2012);
        }

    }
}