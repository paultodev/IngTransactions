using System.Net.Http;
using System.Threading.Tasks;

namespace IngTransactions.Services
{
    public interface IClientService
    {
        Task<string> Authenticate();
    }
}
