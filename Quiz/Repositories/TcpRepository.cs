using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using Quiz.Repositories.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace Quiz.Repositories
{
    public class TcpRepository : ITcpRepository
    {
        public TcpClient Client { get; }

        private IConfiguration _configuration { get; }

        private ILogger<TcpRepository> _logger { get; }

        private bool _disposed = false;

        public TcpRepository(IConfiguration configuration, ILogger<TcpRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;

            this.Client = new TcpClient();

            this.SetupTcpClientConnection();
        }

        public async Task<string> GetValueAsync(int digit)
        {
            var netStream = this.Client.GetStream();
            if (netStream.CanWrite)
            {
                Byte[] sendBytes = Encoding.UTF8.GetBytes(digit.ToString());
                await netStream.WriteAsync(sendBytes, 0, sendBytes.Length);
            }
            if (netStream.CanRead)
            {
                byte[] bytes = new byte[this.Client.ReceiveBufferSize];

                await netStream.ReadAsync(bytes, 0, (int)this.Client.ReceiveBufferSize);
                File.WriteAllBytes("test.txt", bytes);

                return Encoding.Unicode.GetString(bytes);
            }

            netStream.Close();
            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }
            if (disposing)
            {
                this.Client.GetStream().Close();
                this.Client.Close();
                this.Client?.Dispose();
            }

            this._disposed = true;
        }

        private void SetupTcpClientConnection()
        {
            var ip = this._configuration["Connection:Ip"];
            var port = Convert.ToInt32(this._configuration["Connection:Port"]);

            this.Client.Connect(ip, port);
        }
    }
}