using Hostel_Management_Systems.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Text;

namespace Hostel_MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly HttpClient _client;

        public RoomController(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress = new Uri("https://localhost:7255/");
        }

        private void AddToken()
        {
            var token = HttpContext.Session.GetString("token");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                .AuthenticationHeaderValue("Bearer",token.Replace("\"", "").Replace("{token:", "").Replace("}", ""));
        }

        public async Task<IActionResult> RoomIndex()
        {
            AddToken();

            var response = await _client.GetAsync("api/Room");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<List<Room>>(json);

                return View(data);
            }

            return View();
        }

        [HttpGet]
        public IActionResult RoomCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoomCreate(Room vm)
        {
            AddToken();

            var json = JsonConvert.SerializeObject(vm);

            var content = new StringContent(json,Encoding.UTF8,"application/json");

            var response = await _client.PostAsync("api/Room",content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("RoomIndex");
            }

            return View();
        }
    }
}
