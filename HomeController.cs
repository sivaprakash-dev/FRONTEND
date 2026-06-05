using Hostel_Management_Systems.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Hostel_Management_Systems.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;

        public HomeController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress =
                new Uri("https://localhost:7255/api/");
        }

        // ============================
        // REGISTER GET
        // ============================

        [HttpGet]
        public IActionResult AdminRegister()
        {
            return View();
        }

        // ============================
        // REGISTER POST
        // ============================

        [HttpPost]
        public async Task<IActionResult> AdminRegister(AdminReg dto)
        {
            var jsonData =
                JsonConvert.SerializeObject(dto);

            var content =
                new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _client.PostAsync(
                    "Auth/admin-register",
                    content);

            // ERROR MESSAGE READ
            var result =
                await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] =
                    "Registration Successful";

                return RedirectToAction("Login");
            }

            TempData["msg"] = result;

            return View(dto);
        }

        // ============================
        // LOGIN GET
        // ============================

     
    }
}