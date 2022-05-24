using Microsoft.AspNetCore.Mvc;
using VacunassistBackend.Helpers;
using VacunassistBackend.Services;

namespace VacunassistBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IOfficesService _officesService;

        public OfficesController(IOfficesService officesService)
        {
            this._officesService = officesService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new
            {
                offices = _officesService.GetAll()
            });
        }
    }
}