using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Models;

namespace VacunassistBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
    }
}