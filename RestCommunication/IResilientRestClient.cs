using System.Threading.Tasks;
using ServiceDiscovery;

namespace RestCommunication
{
    public interface IResilientRestClient
    {
        Task<TReturnMessage> GetAsync<TReturnMessage>(ServiceEnum serviceName, string path)
            where TReturnMessage : class, new();

        Task<TReturnMessage> PostAsync<TReturnMessage>(ServiceEnum serviceName, string path, object dataObject = null)
            where TReturnMessage : class, new();

        Task<TReturnMessage> PutAsync<TReturnMessage>(ServiceEnum serviceName, string path, object dataObject = null)
            where TReturnMessage : class, new();
         
        Task<bool> DeleteAsync(ServiceEnum serviceName, string path);
    }
}