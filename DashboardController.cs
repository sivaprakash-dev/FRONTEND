using Hostel_Management_Systems.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Hostel_Management_Systems.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _client;

        public DashboardController(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress =
                new Uri("https://localhost:7255/api/");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // SESSION TOKEN

            var token =
                HttpContext.Session
                    .GetString("token");

            // NOT LOGIN

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            // TOKEN SET

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            // API CALL

            var response =
                await _client.GetAsync(
                    "Dashboard");

            // SUCCESS

            if (response.IsSuccessStatusCode)
            {
                var json =
                    await response.Content
                        .ReadAsStringAsync();

                var data =
                    JsonConvert.DeserializeObject<Dashboard>(json);

                return View(data);
            }

            // FAIL

            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account");
        }
    }
}