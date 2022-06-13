using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VacunassistBackend.Models;
using VacunassistBackend.Services;
using VacunassistBackend.Helpers;
using VacunassistBackend.Models.Filters;
using System.Text;
using VacunassistBackend.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace VacunassistBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IVaccinesService _vaccinesService;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersService usersService, IVaccinesService vaccinesService, IConfiguration configuration)
        {
            this._usersService = usersService;
            this._vaccinesService = vaccinesService;
            this._configuration = configuration;
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

        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (_usersService.Exists(request.UserName) == false)
                return BadRequest(new
                {
                    message = "Nombre de usuario no encontrado"
                });

            var user = _usersService.Get(request.UserName);
            var model = new UpdateUserRequest();
            model.Password = RandomGenerator.RandomString(10);
            _usersService.Update(user.Id, model);

            var tempFolder = _configuration["TempFolder"];
            if (Directory.Exists(tempFolder) == false)
                Directory.CreateDirectory(tempFolder);
            var randomName = DateTime.Now.ToString("yyyyMMddHHmmss");
            var path = Path.Combine(tempFolder, randomName + ".txt");
            var text = new StringBuilder();
            text.AppendLine("De: no-reply@vacunassist.com.ar");
            text.AppendLine("A: " + user.Email);
            text.AppendLine("Asunto: Cambio de contraseña");
            text.AppendLine("Su contraseña ha sido cambiada a: " + model.Password);
            System.IO.File.WriteAllText(path, text.ToString());

            return Ok(new
            {
                message = "Contraseña reseteada correctamente"
            });
        }

        [HttpPost]
        [Route("generate-certificate")]
        public IActionResult GenerateCertificate([FromBody] GenerateCertificateRequest request)
        {
            if (_vaccinesService.ExistsApplied(request.Id) == false)
                return BadRequest(new
                {
                    message = "Error, no se encontró la vacuna aplicada"
                });
            GeneratePdf(request);
            return Ok(new
            {
                message = "Contraseña reseteada correctamente"
            });
        }

        private void GeneratePdf(GenerateCertificateRequest request)
        {
            var appliedVaccine = _vaccinesService.GetApplied(request.Id);

            var tempFolder = _configuration["TempFolder"];
            if (Directory.Exists(tempFolder) == false)
                Directory.CreateDirectory(tempFolder);
            var randomName = "certificate_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var path = Path.Combine(tempFolder, randomName + ".pdf");
            var text = new StringBuilder();

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(path, FileMode.Create));

            doc.AddTitle("Certificado de vacunación");
            doc.Open();
            iTextSharp.text.Font _titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _subtitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            var p1 = new Paragraph("VACUNASSIST", _titleFont);
            p1.Alignment = Element.ALIGN_CENTER;
            var p2 = new Paragraph("Certificado de vacunación", _subtitle);
            p2.Alignment = Element.ALIGN_CENTER;
            doc.Add(p1);
            doc.Add(p2);
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("Vacuna: " + appliedVaccine.Vaccine.Name, _standardFont));
            doc.Add(new Paragraph("Paciente: " + appliedVaccine.User.FullName + " (DNI: " + appliedVaccine.User.DNI + ")", _standardFont));
            doc.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), _standardFont));

            doc.Close();
            writer.Close();
        }

        [Helpers.Authorize]
        [HttpPost]
        [Route("{id}/change-password")]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordRequest model)
        {
            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.NewPassword))
                return BadRequest("Invalid Request");

            var user = _usersService.Get(id);
            if (user == null)
                return BadRequest(new
                {
                    message = "El usuario no existe"
                });

            var checkCredentials = _usersService.Authenticate(user.UserName, model.Password);
            if (checkCredentials == null)
            {
                return BadRequest(new
                {
                    message = "La contraseña ingresada es incorrecta"
                });
            }

            _usersService.Update(id, new UpdateUserRequest() { Password = model.NewPassword });

            return Ok(new
            {
                message = "Usuario actualizado correctamente"
            });
        }

        [Helpers.Authorize]
        [HttpPost]
        [Route("{id}/add-vaccine")]
        public IActionResult AddVaccine(int id, [FromBody] AddVaccineRequest model)
        {
            var user = _usersService.Get(id);
            if (user == null)
                return BadRequest(new
                {
                    message = "El usuario no existe"
                });

            _usersService.AddVaccine(id, model);

            return Ok(new
            {
                message = "Usuario actualizado correctamente"
            });
        }

        [Helpers.Authorize]
        [HttpPost]
        [Route("{id}/delete-vaccine")]
        public IActionResult DeleteVaccine(int id, [FromBody] int vaccineId)
        {
            var user = _usersService.Get(id);
            if (user == null)
                return BadRequest(new
                {
                    message = "El usuario no existe"
                });

            _usersService.DeleteVaccine(id, vaccineId);

            return Ok(new
            {
                message = "Usuario actualizado correctamente"
            });
        }

        [HttpGet]
        [Route("profile")]
        [Helpers.Authorize]
        public IActionResult MyProfile()
        {
            var id = User.GetId().Value;
            var user = _usersService.Get(id);
            return Ok(new
            {
                user
            });
        }

        [HttpGet]
        [Route("my-appointments")]
        [Helpers.Authorize]
        public IActionResult MyAppointments()
        {
            var id = User.GetId()!.Value;
            var appointments = _usersService.GetAppointments(id);
            return Ok(new
            {
                appointments
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int? id, [FromBody] UpdateUserRequest model)
        {
            if (id == null)
            {
                return NotFound();
            }

            _usersService.Update(id.Value, model);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] UsersFilterRequest filter)
        {
            return Ok(new
            {
                users = _usersService.GetAll(filter)
            });
        }
    }
}