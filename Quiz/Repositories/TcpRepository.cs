using System;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;


namespace Quiz.Repositories
{
    public class TcpRepository : IDisposable
    {
        private ConfigurationRoot _configuration { get; }

        private bool _disposed = false;

        public TcpClient Client { get; }

        public TcpRepository(IConfigurationRoot configuration)
        {
            this._configuration = (ConfigurationRoot)configuration;
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
                return bytes.ToString();
            }
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