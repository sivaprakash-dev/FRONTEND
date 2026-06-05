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
    public class RoomController : ControllerBase
    {
        private readonly HostelDbContext _context;

        public RoomController(HostelDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET ALL ROOMS
        // =========================================

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var data = await _context.Rooms.ToListAsync();

            return Ok(data);
        }

        // =========================================
        // GET ROOM BY ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var data = await _context.Rooms.FindAsync(id);

            if (data == null)
            {
                return NotFound("Room Not Found");
            }

            return Ok(data);
        }

        // =========================================
        // ADD ROOM
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> AddRoom(Room room)
        {
            room.Status = "Available";

            _context.Rooms.Add(room);

            await _context.SaveChangesAsync();

            return Ok("Room Added Successfully");
        }

        // =========================================
        // UPDATE ROOM
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(
            int id,
            Room room)
        {
            var data = await _context.Rooms.FindAsync(id);

            if (data == null)
            {
                return NotFound("Room Not Found");
            }

            data.RoomNumber = room.RoomNumber;

            data.Capacity = room.Capacity;

            data.OccupiedBeds = room.OccupiedBeds;

            data.Fees = room.Fees;

            data.Status = room.Status;

            await _context.SaveChangesAsync();

            return Ok("Room Updated Successfully");
        }

        // =========================================
        // DELETE ROOM
        // ADMIN ONLY
        // =========================================

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var data = await _context.Rooms.FindAsync(id);

            if (data == null)
            {
                return NotFound("Room Not Found");
            }

            _context.Rooms.Remove(data);

            await _context.SaveChangesAsync();

            return Ok("Room Deleted Successfully");
        }
    }
}

