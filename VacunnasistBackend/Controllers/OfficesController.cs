using Microsoft.AspNetCore.Mvc;
using VacunassistBackend.Helpers;
using VacunassistBackend.Models;
using VacunassistBackend.Models.Filters;
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
        public IActionResult GetAll([FromQuery] OfficesFilterRequest filter)
        {
            return Ok(new
            {
                offices = _officesService.GetAll(filter)
            });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_officesService.Get(id));
        }

        [HttpPost]
        public IActionResult New([FromBody] NewOfficeRequest model)
        {
            var alreadyExist = _officesService.AlreadyExist(model.Name);
            if (alreadyExist)
            {
                return BadRequest(new
                {
                    message = "Ya existe una sede con el mismo nombre"
                });

            }

            _officesService.New(model);

            return Ok(new
            {
                message = "Sede creada correctamente"
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int? id, [FromBody] UpdateOfficeRequest model)
        {
            if (id == null)
            {
                return NotFound();
            }

            _officesService.Update(id.Value, model);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/can-delete")]
        [Helpers.Authorize]
        public IActionResult CanBeDeleted(int id)
        {
            return Ok(_officesService.CanBeDeleted(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var exist = _officesService.Exist(id);
            if (exist == false)
            {
                return BadRequest(new
                {
                    message = "No se pudo encontrar la sede"
                });

            }

            if (_officesService.CanBeDeleted(id) == false)
            {
                return BadRequest(new
                {
                    message = "No se puede eliminar la sede ya que se encuentra relacionada a turnos pendientes/confirmados"
                });
            }

            _officesService.Update(id, new UpdateOfficeRequest() { IsActive = false });

            return Ok(new
            {
                message = "Sede desactivada correctamente"
            });
        }
    }
}