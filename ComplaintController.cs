using Hostel_Management_Systems.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Hostel_MVC.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly HttpClient _client;

        public ComplaintController(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress =
                new Uri("https://localhost:7255/");
        }

        // ============================
        // TOKEN
        // ============================

        private void AddToken()
        {
            var token =
                HttpContext.Session
                    .GetString("token");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders
                    .Authorization =
                    new System.Net.Http.Headers
                    .AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }
        }

        // ============================
        // COMPLAINT LIST
        // ============================

        public async Task<IActionResult> CompIndex()
        {
            AddToken();

            var response =
                await _client.GetAsync(
                    "api/Complaint");

            if (response.IsSuccessStatusCode)
            {
                var json =
                    await response.Content
                        .ReadAsStringAsync();

                var data =
                    JsonConvert.DeserializeObject
                    <List<Complaint>>(json);

                return View(data);
            }

            return View();
        }

        // ============================
        // CREATE PAGE
        // ============================

        [HttpGet]
        public IActionResult CompCreate()
        {
            return View();
        }

        // ============================
        // CREATE COMPLAINT
        // ============================

        [HttpPost]
        public async Task<IActionResult> CompCreate(Complaint vm)
        {
            AddToken();

            var json =
                JsonConvert.SerializeObject(vm);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _client.PostAsync(
                    "api/Complaint",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "CompIndex");
            }

            ViewBag.Error =
                await response.Content
                    .ReadAsStringAsync();

            return View(vm);
        }

        // ============================
        // UPDATE STATUS PAGE
        // ============================

        [HttpGet]
        public IActionResult StatusUpdate(
            int id)
        {
            ViewBag.Id = id;

            return View();
        }

        // ============================
        // UPDATE STATUS
        // ============================

        [HttpPost]
        public async Task<IActionResult> StatusUpdate(
            int id,
            Complaint vm)
        {
            AddToken();

            var json =
                JsonConvert.SerializeObject(vm);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _client.PutAsync(
                    $"api/Complaint/{id}",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "CompIndex");
            }

            return View(vm);
        }

        // ============================
        // DELETE
        // ============================

        [HttpGet]
        public async Task<IActionResult> CompDelete(int id)
        {
            AddToken();

            await _client.DeleteAsync(
                $"api/Complaint/{id}");

            return RedirectToAction(
                "CompIndex");
        }
    }
}