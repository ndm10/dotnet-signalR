using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using signalR.Context;

namespace signalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        
        public UserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            // Find user by id
            var userEntity = await _context.Users.FindAsync(id);
            // If user not found, return 404
            if (userEntity == null)
            {
                return NotFound();
            }

            var type = userEntity.Type;

            var list = await _context.Users.Where(x => x.Type != type).ToListAsync();
            return Ok(list);
        }
    }
}
