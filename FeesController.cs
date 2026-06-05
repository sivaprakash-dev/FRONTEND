using Hostel_Management_System.DTO;
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
    public class FeesController : ControllerBase
    {
        private readonly HostelDbContext _context;

        public FeesController(HostelDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET ALL FEES
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetFees()
        {
            var data = await _context.Fees.ToListAsync();

            return Ok(data);
        }

        // =========================================
        // GET FEE BY STUDENT ID
        // ADMIN + STUDENT
        // =========================================

        [Authorize(Roles = "Admin,Student")]

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetFeeByStudentId(
            int studentId)
        {
            var data = await _context.Fees
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            return Ok(data);
        }

        // =========================================
        // ADD FEE
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> AddFee(Fees fee)
        {
            fee.PaymentStatus = "Pending";

            fee.PaymentDate = DateTime.Now;

            _context.Fees.Add(fee);

            await _context.SaveChangesAsync();

            return Ok("Fee Added Successfully");
        }

        // =========================================
        // PAY FEE
        // STUDENT ONLY
        // =========================================

        [Authorize(Roles = "Student")]

        [HttpPut("{id}")]
        public async Task<IActionResult> PayFee(int id)
        {
            var data = await _context.Fees.FindAsync(id);

            if (data == null)
            {
                return NotFound("Fee Not Found");
            }

            data.PaymentStatus = "Paid";

            data.PaymentDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok("Fee Paid Successfully");
        }

        // =========================================
        // DELETE FEE
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee(int id)
        {
            var data = await _context.Fees.FindAsync(id);

            if (data == null)
            {
                return NotFound("Fee Not Found");
            }

            _context.Fees.Remove(data);

            await _context.SaveChangesAsync();

            return Ok("Fee Deleted Successfully");
        }
    }
}
