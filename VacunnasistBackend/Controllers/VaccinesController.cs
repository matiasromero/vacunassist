using Microsoft.AspNetCore.Mvc;
using VacunassistBackend.Helpers;
using VacunassistBackend.Services;

namespace VacunassistBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IVaccinesService _vaccinesService;

        public VaccinesController(IVaccinesService vaccinesService)
        {
            this._vaccinesService = vaccinesService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new
            {
                vaccines = _vaccinesService.GetAll()
            });
        }
    }
}