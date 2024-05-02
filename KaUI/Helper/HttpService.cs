using KaUI.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace KaUI.Configuration.Helper
{
    public class pdfdto
    {
        public string Html { get; set; }
        public int path { get; set; }
        public string name { get; set; }

    }
    public interface IHttpService
    {
        Task<ApiResult<string>> GetByString(string uri);
        Task<ApiResult<T>> Get<T>(string uri) where T : class;
        Task<ApiResult> Get(string uri);
        Task<ApiResult<T>> Get<T>(string uri, object value) where T : class;
        Task<ApiResult> Post(string uri);
        Task<ApiResult<T>> Post<T>(string uri, object value) where T : class;
        Task<ApiResult<T>> Put<T>(string uri, object value) where T : class;
        Task<FileStream> DownloadFile(string uri);
        Task<Stream> DownloadFileBody(string uri, pdfdto value);
        Task<ApiResult> Delete(string uri);
        Task<string> GetToken();
    }

    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private UrlHelper _urlHelper;
        private HttpClient _httpClient;
        private static string tokerefresh = null;
        private IHttpContextAccessor _httpContext;
        private readonly ILogger<HttpService> _logger;
        private readonly IConfiguration _config;
        public HttpService(
          UrlHelper urlHelper,
          IHttpClientFactory httpClientFactory,
           HttpClient httpClient,
           IHttpContextAccessor httpContext,
        IConfiguration config,
        ILogger<HttpService> logger)
        {
            _urlHelper = urlHelper;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }


        public Task<ApiResult<T>> Get<T>(string url) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return sendRequest<T>(request);
        }
        public Task<ApiResult> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return sendRequest(request);
        }
        public Task<ApiResult<string>> GetByString(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return sendRequest<string>(request);
        }

        public Task<ApiResult<T>> Get<T>(string uri, object value) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return sendRequest<T>(request);
        }
        public Task<ApiResult<T>> Post<T>(string uri, object value) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return sendRequest<T>(request);
        }

        public Task<ApiResult<T>> Put<T>(string uri, object value) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return sendRequest<T>(request);
        }



        public Task<ApiResult> Delete(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            return sendRequest(request);
        }

        private async Task<ApiResult<TData>> sendRequest<TData>(HttpRequestMessage request) where TData : class
        {
            try
            {
                // set the bearer token to the outgoing request as Authentication Header
                // 
                // var accessToken = await RequestClientCredentialsTokenAsync();

                var getToken = await GetToken();
                if (!string.IsNullOrEmpty(getToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", getToken);
                }
                using var response = await _httpClient.SendAsync(request);

                // auto logout on 401 response
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResult<TData>() { data = null, isSuccess = false, message = "" };
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ApiResult<TData>() { data = null, isSuccess = false, message ="" };
                }

                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResult<TData>>();
                    return new ApiResult<TData>() { data = null, isSuccess = false, message = string.Join(",", error.message) };

                }
                var result = await response.Content.ReadFromJsonAsync<ApiResult<TData>>();
                return result;
            }
            catch (Exception e)
            {
                return new ApiResult<TData>() { data = null, isSuccess = false, message = e.Message };

            }

        }

        private async Task<ApiResult> sendRequest(HttpRequestMessage request)
        {

            try
            {

                var getToken = await GetToken();
                if (!string.IsNullOrEmpty(getToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);
                }
                using var response = await _httpClient.SendAsync(request);

                // auto logout on 401 response
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResult() { isSuccess = false, message ="", statusCode = 404 };
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ApiResult() { isSuccess = false, message = "" };
                    return new ApiResult() { isSuccess = false, message = "" };
                }


                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    throw new Exception(error["message"]);
                }
                var result = await response.Content.ReadFromJsonAsync<ApiResult>();
                return result;
            }
            catch (Exception e)
            {
                return new ApiResult() { isSuccess = false, message = e.Message };
            }

        }

        public async Task<FileStream> DownloadFile(string uri)
        {
            var response = await _httpClient.GetAsync(uri);

            

            var getToken = await GetToken();
            if (!string.IsNullOrEmpty(getToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);
            }


            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            FileStream fileStream = null;
            using (Stream input = stream)
            {
                using (Stream memory = CopyToMemory(input))
                {
                    byte[] data = new byte[memory.Length];
                    memory.Read(data, 0, (int)memory.Length);

                    fileStream.Read(data, 0, (int)memory.Length);
                    memory.Close();
                    return fileStream;

                }
            }


        }

        
        public async Task<Stream> DownloadFileBody(string uri, pdfdto value)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            using var response = await _httpClient.SendAsync(request);

            byte[] fileBytes = await _httpClient.GetByteArrayAsync(uri+$"/Getfile?path=/{value.path}/{value.name}.pdf");

            response.EnsureSuccessStatusCode();
            Stream stream = new MemoryStream(fileBytes);
            
            return stream;

            


        }
        private MemoryStream CopyToMemory(Stream input)
        {
            // It won't matter if we throw an exception during this method;
            // we don't *really* need to dispose of the MemoryStream, and the
            // caller should dispose of the input stream
            MemoryStream ret = new MemoryStream();

            byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ret.Write(buffer, 0, bytesRead);
            }
            // Rewind ready for reading (typical scenario)
            ret.Position = 0;
            return ret;
        }
       
        public Task<ApiResult> Post(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            return sendRequest(request);
        }

        public async Task<string> GetToken()
        {
            try
            {
                 var token = _httpContext.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                return token;

            }
            catch (Exception e)
            {
                return null;

            }

            return null;

        }

    }
}
