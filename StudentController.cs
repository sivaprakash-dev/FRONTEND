using Hostel_Management_System.Model;
using Hostel_Management_System.Model.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hostel_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly HostelDbContext _context;

        public StudentController(HostelDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET ALL STUDENTS
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var data = await _context.Students.ToListAsync();
            return Ok(data);
        }

        // =========================================
        // GET STUDENT BY ID
        // ADMIN + STUDENT
        // =========================================

        [Authorize(Roles = "Admin,Student")]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var data = await _context.Students
                .FindAsync(id);

            if (data == null)
            {
                return NotFound("Student Not Found");
            }

            return Ok(data);
        }

        // =========================================
        // ADD STUDENT
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            student.Role = "Student";

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return Ok("Student Added Successfully");
        }

        // =========================================
        // UPDATE STUDENT
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(
            int id,
            Student student)
        {
            var data = await _context.Students
                .FindAsync(id);

            if (data == null)
            {
                return NotFound("Student Not Found");
            }

            data.Name = student.Name;

            data.Gender = student.Gender;

            data.Age = student.Age;

            data.Mobile = student.Mobile;

            data.Email = student.Email;

            data.Address = student.Address;

            data.Photo = student.Photo;

            await _context.SaveChangesAsync();

            return Ok("Student Updated Successfully");
        }

        // =========================================
        // DELETE STUDENT
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var data = await _context.Students
                .FindAsync(id);

            if (data == null)
            {
                return NotFound("Student Not Found");
            }

            _context.Students.Remove(data);

            await _context.SaveChangesAsync();

            return Ok("Student Deleted Successfully");
        }
    }
}