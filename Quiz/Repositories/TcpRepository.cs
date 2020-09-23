using System;
using System.Net.Sockets;

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