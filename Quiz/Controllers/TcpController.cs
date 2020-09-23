using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;

namespace Quiz.Controllers
{
    public class TcpController
    {
        public Dictionary<int, int> Values { get; set; }

        public TcpClient Client { get; set; }

        private ConfigurationRoot _configuration { get; }

        public TcpController(IConfigurationRoot configuration)
        {
            this._configuration = (ConfigurationRoot)configuration;

            this.GenerateValues();
            this.SetUpTcpClient();
        }

        private void GenerateValues()
        {
            var valueSize = Convert.ToInt32(this._configuration["TcpController:ValueSize"]);
            this.Values = Enumerable.Range(1, valueSize).ToDictionary(x => x, x => 0);
        }

        private void SetUpTcpClient()
        {
            var ip = this._configuration["Connection:Ip"];
            var port = Convert.ToInt32(this._configuration["Connection:Port"]);

            this.Client = new TcpClient();
            this.Client.Connect(ip, port);
        }

    }
}