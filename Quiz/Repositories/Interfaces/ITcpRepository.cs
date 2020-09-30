using System;
using System.Threading.Tasks;

namespace Quiz.Repositories.Interfaces
{
    public interface ITcpRepository : IDisposable
    {
         public Task<string> GetValueAsync(int digit);       
    }
}