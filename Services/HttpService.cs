
using Movement_be.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Movement_be.Services
{
    public class HttpService: IHttpService
    {
        //HttpClient _httpClient;

        public HttpService()
        {
            //_httpClient = new HttpClient(new HttpClientHandler
            //{
            //    UseDefaultCredentials = true,
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            //});
        }
        public async Task<string> CallRestApi(HttpMethod httpMethod, string requestUri, string parameterString)
        {
            HttpClient httpClient = new HttpClient(new HttpClientHandler
            {
                UseDefaultCredentials = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (httpClient)
            {
                var request = new HttpRequestMessage(httpMethod, requestUri);
                if (httpMethod == HttpMethod.Post)
                {
                    request.Content = new StringContent(parameterString);
                }

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
    }
}
