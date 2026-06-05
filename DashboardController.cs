using Hostel_Management_System.Model.DbContext;
using Hostel_Management_System.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hostel_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly HostelDbContext _context;

        public DashboardController(HostelDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            Dashboard dashboard = new Dashboard()
            {
                TotalStudents =
                    await _context.Students.CountAsync(),

                TotalRooms =
                    await _context.Rooms.CountAsync(),

                AvailableRooms =
                    await _context.Rooms
                        .CountAsync(x => x.Status == "Available"),

                FullRooms =
                    await _context.Rooms
                        .CountAsync(x => x.Status == "Full"),

                PendingComplaints =
                    await _context.Complaints
                        .CountAsync(x => x.Status == "Pending"),

                ResolvedComplaints =
                    await _context.Complaints
                        .CountAsync(x => x.Status == "Resolved"),

                PendingFees =
                    await _context.Fees
                        .CountAsync(x => x.PaymentStatus == "Pending"),

                PaidFees =
                    await _context.Fees
                        .CountAsync(x => x.PaymentStatus == "Paid")
            };

            return Ok(dashboard);
        }
    }
}