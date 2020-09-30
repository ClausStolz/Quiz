using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using Quiz.Extensions;
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
            try
            {
                var netStream = this.Client.GetStream();
                if (netStream.CanWrite)
                {
                    Byte[] sendBytes = Encoding.UTF8.GetBytes(digit.ToString() + "\n");
                    await netStream.WriteAsync(sendBytes, 0, sendBytes.Length);
                }
                else
                {
                    throw new Exception("Stream can't write to tcp server with index: " + digit.ToString());
                }

                if (netStream.CanRead)
                {
                    byte[] bytes = new byte[this.Client.ReceiveBufferSize];

                    await netStream.ReadAsync(bytes, 0, (int)this.Client.ReceiveBufferSize);
                    File.WriteAllBytes("test.txt", bytes);

                    return Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    throw new Exception("Stream can't read to tcp server with index: " + digit.ToString());
                }
            }
            catch(Exception ex)
            {
                this._logger.WriteLog(ex, "index: " + digit.ToString() + "|" + ex.Message);
                return null;
            }

            
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
            try
            {
                var ip = this._configuration["Connection:Ip"];
                var port = Convert.ToInt32(this._configuration["Connection:Port"]);
                
                this.Client.Connect(ip, port);
                
                if (!this.Client.Connected)
                {
                    throw new Exception("Client doesn't connect to tcp server");
                }

                this._logger.WriteLog("Client connect to tcp server");
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(ex, ex.Message);
            }
            

            
        }
    }
}