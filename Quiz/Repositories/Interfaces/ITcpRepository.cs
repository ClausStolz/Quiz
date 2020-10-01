using System;
using System.Threading.Tasks;

namespace Quiz.Repositories.Interfaces
{
    public interface ITcpRepository
    {
         public Task<string> GetValueAsync(int digit);       
    }
}