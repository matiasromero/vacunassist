using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Models;
using VacunassistBackend.Services;
using VacunnasistBackend.Entities;

namespace VacunassistBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterRequest model)
        {
            if (_usersService.Exists(model.UserName))
                return BadRequest(new
                {
                    message = "Nombre de usuario ya registrado en el sistema"
                });

            var result = _usersService.Register(model);
            if (result)
                return Ok(new
                {
                    message = "Usuario registrado correctamente"
                });

            return BadRequest(new
            {
                message = "Ocurrió un inconveniente al registrar el usuario, intente nuevamente"
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                users = _usersService.GetAll()
            });
        }
    }
}