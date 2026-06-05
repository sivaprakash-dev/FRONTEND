using Hostel_Management_Systems.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Hostel_MVC.Controllers
{
    public class FeesController : Controller
    {
        private readonly HttpClient _client;

        public FeesController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress =
                new Uri("https://localhost:7255/api");
        }

        private void AddToken()
        {
            var token =
                HttpContext.Session
                    .GetString("token");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers
                    .AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }
        }

        // ==========================
        // FEES LIST
        // ==========================

        public async Task<IActionResult> FeesIndex()
        {
            AddToken();

            var response =
                await _client.GetAsync(
                    "api/Fees");

            if (response.IsSuccessStatusCode)
            {
                var json =
                    await response.Content
                        .ReadAsStringAsync();

                var data =
                    JsonConvert.DeserializeObject
                    <List<Fees>>(json);

                return View(data);
            }

            return View();
        }

        // ==========================
        // CREATE PAGE
        // ==========================

        [HttpGet]
        public IActionResult FeesCreate()
        {
            return View();
        }

        // ==========================
        // CREATE
        // ==========================

        [HttpPost]
        public async Task<IActionResult> FeesCreate(
            Fees vm)
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
                    "api/Fees",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "FeesIndex");
            }

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> PayFee(int id)
        {
            AddToken();

            var response =
                await _client.PutAsync(
                    $"api/Fees/{id}",
                    null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // ==========================
        // DELETE
        // ==========================

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id)
        {
            AddToken();

            var response =
                await _client.DeleteAsync(
                    $"api/Fees/{id}");

            return RedirectToAction(
                "FeesIndex");
        }
    }
}