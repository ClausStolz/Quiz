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
                if (!string.IsNullOrEmpty(val))
                {
                    Values[i] = val.GetInt32();
                    if (Values[i] == -1)
                    {
                        this._logger.WriteLog("Not number in string, where index: " + i.ToString());
                    }
                }
            });
            await Task.WhenAll(tasks);
            
            var values = this.Values.Select(x => x.Value);

            if (!values.Contains(-1))
            {
                this._logger.WriteLog("All numbers received, median calculation begins");
                return values.Median();
            }
            else
            {
                this._logger.WriteLog("Non of all numbers received");
                return -1;
            }
            
        }

        private void GenerateValues()
        {
            var valueSize = Convert.ToInt32(this._configuration["TcpController:ValueSize"]);
            this.Values = Enumerable.Range(1, valueSize).ToDictionary(x => x, x => -1);
        }
    }
}