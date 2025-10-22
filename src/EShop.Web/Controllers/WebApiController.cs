using EShop.ViewModels.TestWebApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using EShop.Common.Extensions;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class WebApiController : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUGF5YW0iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiQWRtaW4iLCJDdXN0b21lciJdLCJleHAiOjE3Njg5MzI1OTUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxOTgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTk4In0.6tXCSvjULTSa6ZgNI7T7WiQCL-1FTcYExYjx1MmgQNs"); 
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7198/api/User"),
                Content = new StringContent(string.Empty, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return View("Error");
            }
            string responseBody = await result.Content.ReadAsStringAsync();
            List<ShowUserViewModel>? users = JsonConvert.DeserializeObject<List<ShowUserViewModel>>(responseBody);
            return View(users);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(AddUserViewModel model)
        {
            model.Avatar = await model.UserAvatar.ConvertToBase64();
            model.UserAvatar = null;
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUGF5YW0iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiQWRtaW4iLCJDdXN0b21lciJdLCJleHAiOjE3Njg5MzI1OTUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxOTgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTk4In0.6tXCSvjULTSa6ZgNI7T7WiQCL-1FTcYExYjx1MmgQNs");
            string modelInJson = JsonConvert.SerializeObject(model);
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7198/api/User/Base64"),
                Content = new StringContent(modelInJson, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                ModelState.AddModelError("", "نام کاربری تکراری است");
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            HttpClient client = new();
            string modelInJson = JsonConvert.SerializeObject(model);
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7198/api/Account/login"),
                Content = new StringContent(modelInJson, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            HttpResponseMessage result = await client.SendAsync(request);
            if (!result.IsSuccessStatusCode)
            {
                return Json(new
                {
                    result = false,
                    message = "نام کاربری یا رمز عبور اشتباه است"
                }
                );
            }
            else
            {
                return Json(new
                {
                    result = true
                });
            }
        }
    }
}
