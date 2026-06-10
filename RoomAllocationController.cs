using Hostel_Management_Systems.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Text;

namespace Hostel_MVC.Controllers
{
    public class RoomAllocationController : Controller
    {
        private readonly HttpClient _client;

        public RoomAllocationController(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress = new Uri("https://localhost:7255/");
        }

        private void AddToken()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        }
        public async Task<IActionResult> Indes()
        {
            AddToken();

            var response = await _client.GetAsync("api/RoomAllocation");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<List<RoomAllocation>>(json);

                return View(data);
            }

            return View();
        }

        [HttpGet]
        public IActionResult RoomAllCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoomAllCreate(RoomAllocation vm)
        {
            AddToken();

            var json = JsonConvert.SerializeObject(vm);

            var content = new StringContent(json,Encoding.UTF8,"application/json");

            var response = await _client.PostAsync("api/RoomAllocation",content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Indes");
            }

            var errorMessage = await response.Content.ReadAsStringAsync();

            ViewBag.Error = errorMessage;

            return View(vm);
        }
    }
}
