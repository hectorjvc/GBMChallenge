using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker; 
using ServiceDiscovery;

namespace RestCommunication
{
    public class ResilientRestClient : IRestClient
    {
        // Best practice: Make HttpClient static and reuse.
        // Creating a new instance for each request is an antipattern that can
        // result in socket exhaustion.
        private static readonly HttpClient _client;

        static ResilientRestClient()
        {
            _client = new HttpClient();
            _client.Timeout = _httpTimeOut;
        }

        private readonly Policy _circuitBreakerPolicy;
        //private readonly IConfiguration _configuration;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        // Number of retries
        private readonly int _retryCount = 3;
        // Number of exceptions before opening circuit breaker
        private readonly int _exceptionsAllowedBeforeOpeningCircuit = 6;
        // Timespan before checking circuit
        private readonly TimeSpan _durationOfBreak = TimeSpan.FromSeconds(30);

        // Create a TimeSpan of 4 minutes so that HTTP Calls do not timeout when debugging
        // Do not do this in production!!!
        private static readonly TimeSpan _httpTimeOut = TimeSpan.FromSeconds(240);
        private readonly ILogger _logger;
        private readonly Policy<HttpResponseMessage> _retryPolly;
        private readonly IServiceLocator _serviceLocator;

        public ResilientRestClient(IServiceLocator serviceLocator, ILogger<ResilientRestClient> logger)
        {
            _serviceLocator = serviceLocator;
            _client.Timeout = _httpTimeOut;
            _logger = logger;

            HttpStatusCode[] httpStatusCodesToRetry =
            {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            
            // Configure Retry Policy
            _retryPolly = Policy
                // Specifiy exception types that will be handled. Be careful not to 
                // retry exception rasied by circuit breaker 
                // Don't retry if circuit breaker has broken the circuit
                //.Handle<Exception>(e => !(e is BrokenCircuitException))
                .Handle<TimeoutException>()
                ////.Or<TimeoutException>()
                //.Handle<TimeoutException>()
                .Or<HttpRequestException>()
                .OrResult<HttpResponseMessage>(x => httpStatusCodesToRetry.Contains(x.StatusCode))
                .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry #{retryCount} after a {timeSpan.Seconds} second delay due to error: {exception.Exception.Message}");
                    });

            // Configure Circuit Breaker Pattern
            _circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(
                _exceptionsAllowedBeforeOpeningCircuit,
                _durationOfBreak,
                (ex, breakDelay) =>
                {
                    _logger.LogWarning($"Circuit is 'Open' for {breakDelay.TotalMilliseconds} seconds due to error: {ex.Message}");
                },
                () =>
                {
                    _logger.LogWarning($"Call ok - closing the circuit again");
                },
                () =>
                {
                    _logger.LogWarning($"Circuit is half-open. The next call is a trial");
                });
        }

        public async Task<TReturnMessage> GetAsync<TReturnMessage>(ServiceEnum serviceName, string path)
            where TReturnMessage : class, new()
        {
            // Configure call
            HttpResponseMessage response;
            var result = string.Empty;
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // _client.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            // Execute delegatge. In the event of a retry, this block will re-execute.
            var httpResponse = await HttpInvoker(async () =>
            {
                // Here is actual call to target service              
                response = await _client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    var ex = new HttpRequestException($"{response.StatusCode} -- {response.ReasonPhrase}");
                    // Stuff the Http StatusCode in the Data collection with key 'StatusCode'
                    ex.Data.Add("StatusCode", response.StatusCode);
                    throw ex;
                }

                result = await response.Content.ReadAsStringAsync();

                return response;
            });

            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PostAsync<TReturnMessage>(
            ServiceEnum serviceName,
            string path,
            object dataObject = null) where TReturnMessage : class, new()
        {
            var result = string.Empty;

            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            // Execute delegatge. In the event of a retry, this block will re-execute.
            var httpResponse = await HttpInvoker(async () =>
            {
                var response =
                    await _client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    var ex = new HttpRequestException($"{response.StatusCode} -- {response.ReasonPhrase}");
                    // Stuff the Http StatusCode in the Data collection with key 'StatusCode'
                    ex.Data.Add("StatusCode", response.StatusCode);
                    throw ex;
                }

                result = await response.Content.ReadAsStringAsync();
                return response;
            });

            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PutAsync<TReturnMessage>(ServiceEnum serviceName, string path,
            object dataObject = null)
            where TReturnMessage : class, new()
        {
            var result = string.Empty;

            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            // Execute delegatge. In the event of a retry, this block will re-execute.
            var httpResponse = await HttpInvoker(async () =>
            {
                var response =
                    await _client.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    var ex = new HttpRequestException($"{response.StatusCode} -- {response.ReasonPhrase}");
                    // Stuff the Http StatusCode in the Data collection with key 'StatusCode'
                    ex.Data.Add("StatusCode", response.StatusCode);
                    throw ex;
                }

                result = await response.Content.ReadAsStringAsync();
                return response;
            });

            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<bool> DeleteAsync(ServiceEnum serviceName, string path)
        {
            HttpResponseMessage response = null;


            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");
            //_logger.LogInformation("[INFO] DELETE Uri:" + uri);

            // Execute delegatge. In the event of a retry, this block will re-execute.
            var httpResponse = await HttpInvoker(async () =>
            {

                response = await _client.DeleteAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    var ex = new HttpRequestException($"{response.StatusCode} -- {response.ReasonPhrase}");
                    // Stuff the Http StatusCode in the Data collection with key 'StatusCode'
                    ex.Data.Add("StatusCode", response.StatusCode);
                    throw ex;
                }
                return response;
            });

            return response.IsSuccessStatusCode;
        }
        
        private async Task<HttpResponseMessage> HttpInvoker(Func<Task<HttpResponseMessage>> operation)
        {
            return await _retryPolly.ExecuteAsync(() => _circuitBreakerPolicy.ExecuteAsync(operation));
        }
    }
}

#region Extra Code
//private void InitializePolly()
//{
//    HttpStatusCode[] httpStatusCodesToRetry =
//    {
//        HttpStatusCode.RequestTimeout, // 408
//        HttpStatusCode.InternalServerError, // 500
//        HttpStatusCode.BadGateway, // 502
//        HttpStatusCode.ServiceUnavailable, // 503
//        HttpStatusCode.GatewayTimeout // 504
//    };

//    retryPolicy = Policy
//        .Handle<TimeoutException>()
//        .Or<HttpRequestException>()
//        .OrResult<HttpResponseMessage>(r => httpStatusCodesToRetry.Contains(r.StatusCode))
//        .WaitAndRetryAsync(3,
//            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
//            (response, delay, retryCount, context) =>
//            {
//                Debug.WriteLine($"Retry {retryCount} after {delay.Seconds} seconds delay due to {response.Exception.Message}");
//            });
//}
#endregion

