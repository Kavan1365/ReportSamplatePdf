using KaUI.Configuration.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;

namespace KaUI.Configuration
{
    public class DataSourceResult<TDestination> : ActionResult where TDestination : class
    {

        private readonly IHttpService _httpService;
        private IHttpContextAccessor httpContextAccessor;
        private HttpClient _httpClient;
        private string _url;

        public DataSourceResult(string url, IHttpContextAccessor accessor, IHttpService httpService)
        {
            _httpService = httpService;
            httpContextAccessor = accessor;
            _httpClient = new HttpClient();
            _url = url;
        }


        public override async Task ExecuteResultAsync(ActionContext context)
        {


            var queryString = (System.Uri.UnescapeDataString(context.HttpContext.Request.QueryString.Value)).Replace("\\", "").Replace("?", "");
            var qurestringCount = queryString.Length;
            var indexFirst = queryString.IndexOf("{");
            if (indexFirst > 0)
                queryString = queryString.Substring(indexFirst, qurestringCount - indexFirst);
            var index = queryString.IndexOf("&_=");
            if (index > 0)
                queryString = queryString.Substring(0, index);


            var request = new HttpRequestMessage(HttpMethod.Post, _url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(queryString);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var accessToken = await _httpService.GetToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();




            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResultCustome<TDestination>>(content);


            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            });
            try
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await base.ExecuteResultAsync(context);

                }

                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    throw new Exception(error["message"] + " " + response.IsSuccessStatusCode);
                }
                using (var sw = new StringWriter())
                {
                    jsonSerializer.Serialize(sw, result);

                    await context.HttpContext.Response.WriteAsync(sw.ToString(), cancellationToken: CancellationToken.None);
                }

                await base.ExecuteResultAsync(context);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " token=" + accessToken);
            }

        }


        




    }
    public static class PdfService
    {
    public static async Task<ApiResultCustomeForPdf<TDestination>> ExecuteResultToPdfAsync<TDestination>(string _url, string queryString, IHttpService _httpService, HttpClient _httpClient) where TDestination : class
        {


        var qurestringCount = queryString.Length;
        var indexFirst = queryString.IndexOf("{");
        if (indexFirst > 0)
            queryString = queryString.Substring(indexFirst, qurestringCount - indexFirst);
        var index = queryString.IndexOf("&_=");
        if (index > 0)
            queryString = queryString.Substring(0, index);


        var request = new HttpRequestMessage(HttpMethod.Post, _url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = new StringContent(queryString);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


        var accessToken = await _httpService.GetToken();
        if (!string.IsNullOrEmpty(accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();


        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResultCustomeForPdf<TDestination>>(content);


        return result;


    }
    }
}
