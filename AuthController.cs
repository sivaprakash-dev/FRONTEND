using Hostel_Management_System.DTO;
using Hostel_Management_System.Model;
using Hostel_Management_System.Model.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hostel_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HostelDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(HostelDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        [HttpPost("admin-register")]
        public async Task<IActionResult> AdminRegister(AdminRegDto dto)
        {
            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingAdmin != null)
            {
                return BadRequest("Admin already exists");
            }

            Admin admin = new Admin()
            {
                Name = dto.Name,

                Email = dto.Email,

                Gender = dto.Gender,

                Age = dto.Age,

                Address = dto.Address,

                Password = dto.Password,

                Mobile = dto.Mobile,

                JoiningDate = dto.JoiningDate,

                Photo = dto.Photo,

                Role = "Admin"
            };

            _context.Admins.Add(admin);

            await _context.SaveChangesAsync();

            return Ok("Admin Registration Successful");
        }

        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin(AdminLoginDto dto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync
            (x =>x.Email == dto.Email && x.Password == dto.Password);

            if (admin == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, admin.Email!),

        new Claim(ClaimTypes.Role, admin.Role!)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"],

                audience: _configuration["Jwt:Audience"],

                claims: claims,

                expires: DateTime.Now.AddHours(5),

                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        
        [HttpPost("student-register")]
        public async Task<IActionResult> StudentRegister(StudentRegDto dto)
        {
            var existingStudent = await _context.Students.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingStudent != null)
            {
                return BadRequest("Student already exists");
            }

            Student student = new Student()
            {
                Name = dto.Name,

                Gender = dto.Gender,

                Age = dto.Age,

                Mobile = dto.Mobile,

                Email = dto.Email,

                Address = dto.Address,

                Photo = dto.Photo,

                Password = dto.Password,

                Role = "Student"
            };

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return Ok("Student Registration Successful");
        }
        
        [HttpPost("student-login")]
        public async Task<IActionResult> StudentLogin(StudentLoginDto dto)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.Email == dto.Email && x.Password == dto.Password);

            if (student == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, student.Email!),

                new Claim(ClaimTypes.Role, "Student"),

                new Claim("StudentId",student.StudentId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"],

                audience: _configuration["Jwt:Audience"],

                claims: claims,

                expires: DateTime.Now.AddHours(2),

                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var data = await _context.Students.ToListAsync();

            return Ok(data);
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var data = await _context.Students
                .FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }
        
        [Authorize(Roles = "Student")]
        [HttpGet("dashboard")]
        public IActionResult StudentDashboard()
        {
            return Ok("Welcome Student");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-dashboard")]
        public IActionResult AdminDashboard()
        {
            return Ok(new
            {
                Message = "Welcome Admin"
            });
        }
    }
}

