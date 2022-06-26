using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VacunassistBackend.Entities;
using VacunassistBackend.Helpers;
using VacunassistBackend.Models;
using VacunassistBackend.Models.Filters;
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

        [HttpPost]
        [Route("confirmed")]
        public IActionResult NewConfirmedAppointment([FromBody] NewConfirmedAppointmentRequest model)
        {
            if (model.CurrentId.HasValue == false)
            {
                var alreadyExist = _appointmentsService.AlreadyExist(model.PatientId, model.VaccineId);
                if (alreadyExist)
                {
                    return BadRequest(new
                    {
                        message = "El usuario ya tiene un turno o solicitud pendiente para esta vacuna"
                    });
                }
            }

            _appointmentsService.AddConfirmed(model);

            return Ok(new
            {
                message = "Turno creado correctamente"
            });
        }

        [HttpGet]
        public IActionResult Get([FromQuery] AppointmentsFilterRequest filter)
        {
            //var id = User.GetId()!.Value;
            var role = User.GetRole()!;
            var appointments = _appointmentsService.GetAll(filter);
            if (role == UserRoles.Vacunator)
            {
                appointments = appointments.Where(x => x.Vaccinator != null && x.Vaccinator.Id == User.GetId()!.Value).ToArray();
            }
            var result = appointments.Select(x => new AppointmentModel()
            {
                Id = x.Id,
                AppliedDate = x.AppliedDate,
                Date = x.Date,
                Comment = x.Comment,
                Notified = x.Notified,
                PatientId = x.Patient.Id,
                PatientName = x.Patient.FullName,
                PreferedOfficeId = x.PreferedOffice?.Id,
                PreferedOfficeName = x.PreferedOffice?.Name,
                PreferedOfficeAddress = x.PreferedOffice?.Address,
                RequestedAt = x.RequestedAt,
                Status = x.Status,
                VaccineId = x.Vaccine.Id,
                VaccineName = x.Vaccine.Name,
                VaccinatorId = x.Vaccinator?.Id,
                VaccinatorName = x.Vaccinator?.FullName,

            }).ToArray();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var a = _appointmentsService.Get(id);
            var result = new AppointmentModel()
            {
                Id = a.Id,
                AppliedDate = a.AppliedDate,
                Date = a.Date,
                Comment = a.Comment,
                Notified = a.Notified,
                PatientId = a.Patient.Id,
                PatientName = a.Patient.FullName,
                PatientAge = a.Patient.GetAge(),
                PatientRisk = a.Patient.BelongsToRiskGroup,
                PreferedOfficeId = a.PreferedOffice?.Id,
                PreferedOfficeName = a.PreferedOffice?.Name,
                PreferedOfficeAddress = a.PreferedOffice?.Address,
                RequestedAt = a.RequestedAt,
                Status = a.Status,
                VaccineId = a.Vaccine.Id,
                VaccineName = a.Vaccine.Name,
                VaccinatorId = a.Vaccinator?.Id,
                VaccinatorName = a.Vaccinator?.FullName,

            };
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var exist = _appointmentsService.Exist(id);
            if (exist == false)
            {
                return BadRequest(new
                {
                    message = "No se pudo encontrar el turno a cancelar"
                });

            }

            _appointmentsService.Update(id, new UpdateAppointmentRequest() { Status = AppointmentStatus.Cancelled });

            return Ok(new
            {
                message = "Turno cancelado correctamente"
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int? id, [FromBody] UpdateAppointmentRequest model)
        {
            if (id == null)
            {
                return NotFound();
            }

            _appointmentsService.Update(id.Value, model);
            return Ok();
        }
    }
}