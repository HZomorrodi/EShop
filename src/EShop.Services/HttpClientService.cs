using EShop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services
{
    public class HttpClientService(HttpClient httpClient) : IHttpClientService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, string? authorizationToken = null, string content = "", string mediaType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
            }
            HttpRequestMessage request = new()
            {
                Method = method,
                RequestUri = new Uri(url),
                Content = new StringContent(content, Encoding.UTF8, mediaType)
            };
            return await _httpClient.SendAsync(request);
        }
    }
}
