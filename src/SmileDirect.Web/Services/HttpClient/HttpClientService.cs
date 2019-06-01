using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmileDirect.Web.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }

    public class HttpClientService : IHttpClientService
    {
        private HttpClient Client { get; set; }

        public HttpClientService(HttpClient client)
        {
            Client = client;
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return Client.GetAsync(requestUri);
        }
    }
}