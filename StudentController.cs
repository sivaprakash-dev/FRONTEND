using Hostel_Management_Systems.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Text;

namespace Hostel_MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _client;

        public StudentController(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient();

            _client.BaseAddress = new Uri("https://localhost:7255/");
        }

        private void AddToken()
        {
            var token = HttpContext.Session.GetString("token");

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Bearer",token.Replace("\"", "").Replace("{token:", "").Replace("}", ""));
        }

        public async Task<IActionResult> Indexs(string search,int page = 1)
        {
            AddToken();

            var response = await _client.GetAsync("api/Student");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Student>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var students = JsonConvert.DeserializeObject<List<Student>>(json);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                students = students.Where(x =>

                    (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(search))

                    ||

                    (!string.IsNullOrEmpty(x.Gender) && x.Gender.ToLower().Contains(search))

                    ||

                    x.Age.ToString().Contains(search)

                    ||

                    (!string.IsNullOrEmpty(x.Mobile) && x.Mobile.Contains(search))

                    ||

                    (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(search))

                ).ToList();
            }

            int pageSize = 5;

            int totalRecords = students.Count();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            students = students.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Student vm)
        {
            AddToken();

            var json = JsonConvert.SerializeObject(vm);

            var content = new StringContent(json,Encoding.UTF8,"application/json");

            var response = await _client.PostAsync("api/Student",content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Indexs");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AddToken();

            var response = await _client.GetAsync($"api/Student/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<Student>(json);

                return View(data);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,Student vm)
        {
            AddToken();

            var json = JsonConvert.SerializeObject(vm);

            var content = new StringContent(json,Encoding.UTF8,"application/json");

            var response = await _client.PutAsync($"api/Student/{id}",content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Indexs");
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AddToken();

            var response = await _client.DeleteAsync($"api/Student/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Indexs");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            AddToken();

            var response = await _client.GetAsync($"api/Student/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<Student>(json);

                return View(data);
            }

            return View();
        }
    }
}
