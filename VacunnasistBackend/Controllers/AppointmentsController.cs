using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VacunassistBackend.Entities;
using VacunassistBackend.Helpers;
using VacunassistBackend.Models;
using VacunassistBackend.Services;

namespace VacunassistBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;
        private readonly IAppointmentsService _appointmentsService;
        private readonly IConfiguration _configuration;

        public AppointmentsController(IUsersService usersService, IAppointmentsService appointmentsService, IConfiguration configuration)
        {
            this._usersService = usersService;
            this._appointmentsService = appointmentsService;
            this._configuration = configuration;
        }

        [HttpPost]
        public IActionResult NewAppointment([FromBody] NewAppointmentRequest model)
        {
            var userId = User.GetId()!.Value;

            var alreadyExist = _appointmentsService.AlreadyExist(userId, model.VaccineId);
            if (alreadyExist)
            {
                return BadRequest(new
                {
                    message = "El usuario ya tiene un turno o solicitud pendiente para esta vacuna"
                });

            }

            _appointmentsService.Add(userId, model.VaccineId);

            return Ok(new
            {
                message = "Turno creado correctamente"
            });
        }
    }
}