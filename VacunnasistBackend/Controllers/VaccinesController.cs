using System.Globalization;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Helpers;
using VacunassistBackend.Infrastructure;
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
        private readonly IConfiguration _configuration;

        public VaccinesController(DataContext context, IVaccinesService vaccinesService, IConfiguration configuration)
        {
            this._vaccinesService = vaccinesService;
            this._configuration = configuration;
            this._context = context;
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

        [HttpPost]
        public IActionResult New([FromBody] NewVaccineRequest model)
        {
            var alreadyExist = _vaccinesService.AlreadyExist(model.Name);
            if (alreadyExist)
            {
                return BadRequest(new
                {
                    message = "Ya existe una vacuna con el mismo nombre"
                });

            }

            _vaccinesService.New(model);

            return Ok(new
            {
                message = "Vacuna creada correctamente"
            });
        }

        [HttpPost]
        [Route("report-vaccines")]
        public IActionResult ReportVaccines()
        {
            var date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var applieds = _context.AppliedVaccines.Include(u => u.Vaccine)
            .Where(x => x.Appointment != null && x.AppliedDate.HasValue && x.AppliedDate > firstDayOfMonth && x.AppliedDate < lastDayOfMonth)
            .ToArray();
            if (applieds.Any() == false)
                throw new HttpResponseException(400, message: "No se han aplicado vacunas este mes");

            var grouped = applieds.GroupBy(x => x.Vaccine.Id).OrderBy(x => x.Key).ToArray();

            iTextSharp.text.Font _titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _subtitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            var tempFolder = _configuration["TempFolder"];
            if (Directory.Exists(tempFolder) == false)
                Directory.CreateDirectory(tempFolder);

            var randomName = "report_vaccines_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var path = Path.Combine(tempFolder, randomName + ".pdf");
            var text = new StringBuilder();

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(path, FileMode.Create));
            doc.AddTitle("Reporte de dosis");
            doc.Open();

            var currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            var p1 = new Paragraph("VACUNASSIST", _titleFont);
            p1.Alignment = Element.ALIGN_CENTER;
            var p2 = new Paragraph("Reporte de dosis por vacunas en el mes actual (" + currentMonth + ")", _subtitle);
            p2.Alignment = Element.ALIGN_CENTER;

            doc.Add(p1);
            doc.Add(p2);
            doc.Add(Chunk.NEWLINE);

            List list = new List(List.UNORDERED, 20f);
            list.IndentationLeft = 20f;
            list.PreSymbol = "*";
            foreach (var g in grouped)
            {
                list.Add(string.Format("Vacuna: {0} - {1} dosis aplicadas", g.First().Vaccine.Name, g.Count()));
            }

            doc.Add(list);
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("Fecha reporte: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), _standardFont));
            doc.Add(Chunk.NEWLINE);
            var barcode = new BarcodeQRCode("www.vacunassist.com", 100, 100, null);
            iTextSharp.text.Image imgBarCode = barcode.GetImage();
            imgBarCode.SetAbsolutePosition(483, 740);
            doc.Add(imgBarCode);

            doc.Close();
            writer.Close();

            return Ok(new
            {
                message = "Reporte creado correctamente"
            });
        }

        [HttpPost]
        [Route("report-patients")]
        public IActionResult ReportPatients()
        {
            var date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var applieds = _context.AppliedVaccines.Include(u => u.Vaccine)
            .Where(x => x.Appointment != null && x.AppliedDate.HasValue && x.AppliedDate > firstDayOfMonth && x.AppliedDate < lastDayOfMonth)
            .ToArray();
            if (applieds.Any() == false)
                throw new HttpResponseException(400, message: "No se han aplicado vacunas este mes");

            var grouped = applieds.GroupBy(x => x.AppliedDate.Value.Date).OrderBy(x => x.Key).ToArray();

            iTextSharp.text.Font _titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _subtitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            var tempFolder = _configuration["TempFolder"];
            if (Directory.Exists(tempFolder) == false)
                Directory.CreateDirectory(tempFolder);

            var randomName = "report_patients_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var path = Path.Combine(tempFolder, randomName + ".pdf");
            var text = new StringBuilder();

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(path, FileMode.Create));
            doc.AddTitle("Reporte de pacientes");
            doc.Open();

            var currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            var p1 = new Paragraph("VACUNASSIST", _titleFont);
            p1.Alignment = Element.ALIGN_CENTER;
            var p2 = new Paragraph("Reporte de vacunados/as por fecha en el mes actual (" + currentMonth + ")", _subtitle);
            p2.Alignment = Element.ALIGN_CENTER;

            doc.Add(p1);
            doc.Add(p2);
            doc.Add(Chunk.NEWLINE);

            List list = new List(List.UNORDERED, 20f);
            list.IndentationLeft = 20f;
            list.PreSymbol = "*";
            foreach (var g in grouped)
            {
                list.Add(string.Format("{0:dd} de {1} - {2} aplicadas", g.Key, currentMonth, g.GroupBy(x => x.User).Count()));
            }

            doc.Add(list);
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("Fecha reporte: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), _standardFont));
            doc.Add(Chunk.NEWLINE);
            var barcode = new BarcodeQRCode("www.vacunassist.com", 100, 100, null);
            iTextSharp.text.Image imgBarCode = barcode.GetImage();
            imgBarCode.SetAbsolutePosition(483, 740);
            doc.Add(imgBarCode);

            doc.Close();
            writer.Close();

            return Ok(new
            {
                message = "Reporte creado correctamente"
            });
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