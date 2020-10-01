using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Quiz.Managers;
using Quiz.Extensions;
using Quiz.Repositories.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace Quiz.Controllers
{
    public class TcpController
    {
        public Dictionary<string, int> Values { get; set; }

        public bool IsFinished { get; set; }

        private IConfiguration _configuration { get; }

        private ITcpRepository _tcpRepository { get; }

        private ILogger<TcpController> _logger { get; }

        public TcpController(IConfiguration configuration, ILogger<TcpController> logger, ITcpRepository tcpRepository)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._tcpRepository = tcpRepository;

            this.IsFinished = false;

            this.Values = SaveValueManager.LoadValues(Convert.ToInt32(this._configuration["TcpController:ValueSize"]));

            this._logger.WriteLog("Create TcpController");
        }

        public async Task<decimal> GetMedian()
        {
            var valueSize = Values.Count;
            var tasks = Enumerable.Range(1, valueSize)
            .Select(async i =>
            {
                var txt_i = i.ToString();
                if (Values[txt_i] == -1)
                {
                    var val = await _tcpRepository.GetValueAsync(i);
                    if (!string.IsNullOrEmpty(val))
                    {
                        Values[txt_i] = val.GetInt32();
                        if (Values[txt_i] == -1)
                        {
                            this._logger.WriteLog("Not number in string, where index: " + txt_i);
                        }
                    }   
                }   
            });
            await Task.WhenAll(tasks);
            
            SaveValueManager.SaveValues(this.Values);
            var values = this.Values.Select(x => x.Value);

            if (!values.Contains(-1))
            {
                this._logger.WriteLog("All numbers received, median calculation begins");
                IsFinished = true;
                return values.Median();
            }
            else
            {
                this._logger.WriteLog("Non of all numbers received");
                return -1;
            }
            
        }
    }
}