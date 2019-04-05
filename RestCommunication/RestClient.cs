using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceDiscovery;

namespace RestCommunication
{
    public class RestClient : IRestClient
    {
        // Best pracitce: Make HttpClient static and reuse.
        // Creating a new instance for each request is an antipattern that can
        // result in socket exhaustion.
        private static readonly HttpClient _client; 

        static RestClient()
        {
            _client =  new HttpClient();
            _client.Timeout = _httpTimeOut;
        }

        // Create a TimeSpan of 4 minutes so that HTTP Calls do not timeout when debugging
        // Do not do this in production!!!
        private static readonly TimeSpan _httpTimeOut = new TimeSpan(0, 4, 0);
        private readonly IServiceLocator _serviceLocator;

        public RestClient(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
             //_logger = LogFactory.GetLogInstance<RestClient>();
        }

        public async Task<TReturnMessage> GetAsync<TReturnMessage>(ServiceEnum serviceName, string path)
            where TReturnMessage : class, new()
        {
            HttpResponseMessage response;

            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // _client.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            // Here is actual call to target service              
            response = await _client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                var ex = new HttpRequestException($"{response.StatusCode} -- {response.ReasonPhrase}");
                // Stuff the Http StatusCode in the Data collection with key 'StatusCode'
                ex.Data.Add("StatusCode", response.StatusCode);
                throw ex;
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PostAsync<TReturnMessage>(
            ServiceEnum serviceName,
            string path,
            object dataObject = null) where TReturnMessage : class, new()

        {
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            var response = await _client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PutAsync<TReturnMessage>(ServiceEnum serviceName, string path,
            object dataObject = null)
            where TReturnMessage : class, new()
        {
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            var response = await _client.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<bool> DeleteAsync(ServiceEnum serviceName, string path)
        {
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");
            //_logger.LogInformation("[INFO] DELETE Uri:" + uri);

            var response = await _client.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
    }
}