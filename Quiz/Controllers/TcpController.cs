using System;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Generic;

using Quiz.Repositories;
using Microsoft.Extensions.Configuration;


namespace Quiz.Controllers
{
    public class TcpController
    {
        public Dictionary<int, int> Values { get; set; }

        private ConfigurationRoot _configuration { get; }

        private TcpRepository _tcpRepository { get; }

        public TcpController(IConfigurationRoot configuration)
        {
            this._configuration = (ConfigurationRoot)configuration;
            this._tcpRepository = new TcpRepository(configuration);
            
            this.GenerateValues();
        }
        private void GenerateValues()
        {
            var valueSize = Convert.ToInt32(this._configuration["TcpController:ValueSize"]);
            this.Values = Enumerable.Range(1, valueSize).ToDictionary(x => x, x => 0);
        }
    }
}