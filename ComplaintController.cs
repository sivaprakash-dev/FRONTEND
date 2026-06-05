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
    public class ComplaintController : ControllerBase
    {
        private readonly HostelDbContext _context;

        public ComplaintController(HostelDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET ALL COMPLAINTS
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetComplaints()
        {
            var data = await _context.Complaints
                .ToListAsync();

            return Ok(data);
        }

        // =========================================
        // ADD COMPLAINT
        // STUDENT ONLY
        // =========================================

        [Authorize(Roles = "Admin,Student")]

        [HttpPost]
        public async Task<IActionResult> AddComplaint(
            Complaint complaint)
        {
            complaint.Status = "Pending";

            complaint.CreatedDate = DateTime.Now;

            _context.Complaints.Add(complaint);

            await _context.SaveChangesAsync();

            return Ok("Complaint Added Successfully");
        }

        // =========================================
        // UPDATE STATUS
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComplaint(
            int id,
            Complaint complaint)
        {
            var data = await _context.Complaints
                .FindAsync(id);

            if (data == null)
            {
                return NotFound("Complaint Not Found");
            }

            data.Status = complaint.Status;

            await _context.SaveChangesAsync();

            return Ok("Complaint Status Updated");
        }

        // =========================================
        // DELETE COMPLAINT
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(
            int id)
        {
            var data = await _context.Complaints
                .FindAsync(id);

            if (data == null)
            {
                return NotFound("Complaint Not Found");
            }

            _context.Complaints.Remove(data);

            await _context.SaveChangesAsync();

            return Ok("Complaint Deleted Successfully");
        }

    }
}
