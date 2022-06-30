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
    public class VaccinesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IVaccinesService _vaccinesService;

        public VaccinesController(IVaccinesService vaccinesService)
        {
            this._vaccinesService = vaccinesService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] VaccinesFilterRequest filter)
        {
            return Ok(new
            {
                vaccines = _vaccinesService.GetAll(filter)
            });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_vaccinesService.Get(id));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int? id, [FromBody] UpdateVaccineRequest model)
        {
            if (id == null)
            {
                return NotFound();
            }

            _vaccinesService.Update(id.Value, model);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/can-delete")]
        [Helpers.Authorize]
        public IActionResult CanBeDeleted(int id)
        {
            return Ok(_vaccinesService.CanBeDeleted(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var exist = _vaccinesService.Exist(id);
            if (exist == false)
            {
                return BadRequest(new
                {
                    message = "No se pudo encontrar la vacuna"
                });

            }

            if (_vaccinesService.CanBeDeleted(id) == false)
            {
                return BadRequest(new
                {
                    message = "No se puede eliminar la vacuna ya que se encuentra relacionada a turnos pendientes/confirmados"
                });
            }

            _vaccinesService.Update(id, new UpdateVaccineRequest() { IsActive = false });

            return Ok(new
            {
                message = "Vacuna desactivada correctamente"
            });
        }
    }
}