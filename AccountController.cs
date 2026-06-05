using Hostel_Management_Systems.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Text;

namespace Hostel_Management_Systems.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;

        public AccountController(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress =
                new Uri("https://localhost:7255/");
        }

        // =========================================
        // LOGIN PAGE
        // =========================================

        [HttpGet]
        public IActionResult Login()
        {
            var token =
                HttpContext.Session
                    .GetString("token");

            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            return View();
        }

        // =========================================
        // LOGIN
        // =========================================

        [HttpPost]
        public async Task<IActionResult> Login(
            AdminLogin vm)
        {
            var json =
                JsonConvert.SerializeObject(vm);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _client.PostAsync(
                    "api/Auth/admin-login",
                    content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var data =
                    JsonConvert.DeserializeObject<LoginResponse>(result);

                HttpContext.Session.SetString("token", data!.Token!);

                // Login User Email Save
                HttpContext.Session.SetString("Email", vm.Email!);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Message =
                "Invalid Email or Password";

            return View();
        }

        // =========================================
        // LOGOUT
        // =========================================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account");
        }
    }
}