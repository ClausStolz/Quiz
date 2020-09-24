using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Quiz.Extensions;
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

        public decimal GetMedian()
        {
            List<Task> tasks = new List<Task>();

            var  valueSize = Values.Count;
            for (int i = 1; i <= valueSize; i++)
            {
                tasks.Add(Task.Factory.StartNew(
                    () => {
                        this.Values[i] = Convert.ToInt32(this._tcpRepository.GetValueAsync(i));
                    }
                ));
            }
            Task.WaitAll(tasks.ToArray());
            
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