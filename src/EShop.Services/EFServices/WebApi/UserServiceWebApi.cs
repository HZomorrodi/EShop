using EShop.Services.Contracts;
using EShop.Services.Contracts.WebApi;
using EShop.ViewModels.TestWebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices.WebApi
{
    public class UserServiceWebApi(IHttpClientService httpClientService, ICookieManager cookieManager) : IUserServiceWebApi
    {
        private readonly IHttpClientService _httpClientService = httpClientService;
        private readonly ICookieManager cookieManager = cookieManager;

        public async Task<OperationResult<List<ShowUserViewModel?>>> GetAllUserAsync()
        {
            var result = await _httpClientService.SendAsync("https://localhost:7198/api/User", HttpMethod.Get, cookieManager.GetValue("JWTToken"));

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new OperationResult<List<ShowUserViewModel?>>(false, null);
            }
            string responseBody = await result.Content.ReadAsStringAsync();
            List<ShowUserViewModel?> users = JsonConvert.DeserializeObject<List<ShowUserViewModel?>>(responseBody);
            return new OperationResult<List<ShowUserViewModel?>>(true, users);
        }

        public async Task<OperationResult<string>> AddAsync(AddUserViewModel input)
        {
            string modelInJson = JsonConvert.SerializeObject(input);
            var result = await _httpClientService.SendAsync("https://localhost:7198/api/User/Base64", HttpMethod.Post, cookieManager.GetValue("JWTToken"), modelInJson);

            if (result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return new OperationResult<string>(false, "نام کاربری تکراری است");
            }
            return new OperationResult<string>(true, null);
        }
        public async Task<OperationResult<string>> Login(LoginViewModel input)
        {
            string modelInJson = JsonConvert.SerializeObject(input);
            HttpResponseMessage result = await _httpClientService.SendAsync("https://localhost:7198/api/Account/login", HttpMethod.Post, content: modelInJson);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new OperationResult<string>(false, "نام کاربری یا رمز عبور اشتباه است");
            }
            string responseBody = await result.Content.ReadAsStringAsync();
            return new OperationResult<string>(true, responseBody);
        }
    }
}
