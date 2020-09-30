using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Quiz.Extensions;
using Quiz.Repositories;
using Quiz.Repositories.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace Quiz.Controllers
{
    public class TcpController
    {
        public Dictionary<int, int> Values { get; set; }

        private IConfiguration _configuration { get; }

        private ITcpRepository _tcpRepository { get; }

        private ILogger<TcpController> _logger { get; }

        public TcpController(IConfiguration configuration, ILogger<TcpController> logger, ITcpRepository tcpRepository)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._tcpRepository = tcpRepository;

            this.GenerateValues();

            this._logger.WriteLog("Create TcpController");
        }

        public async Task<decimal> GetMedian()
        {
            var valueSize = Values.Count;
            var tasks = Enumerable.Range(1, valueSize)
            .Select(async i =>
            {
                string val = await _tcpRepository.GetValueAsync(i);
                Values[i] = val.GetInt32();
            });
            await Task.WhenAll(tasks);
            
            var values = this.Values.Select(x => x.Value);
            return values.Median();
        }

        private void GenerateValues()
        {
            var valueSize = Convert.ToInt32(this._configuration["TcpController:ValueSize"]);
            this.Values = Enumerable.Range(1, valueSize).ToDictionary(x => x, x => 0);
        }
    }
}