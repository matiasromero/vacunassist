using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Services
{
    public interface INotificationsService
    {
        void Trigger();
        void Trigger(int id);
        void SendCancellation(Appointment a);
    }

    public class NotificationsService : INotificationsService
    {
        private DataContext _context;
        private readonly IConfiguration _configuration;


        public NotificationsService(DataContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }


        public void Trigger()
        {
            DoTrigger(new int[] { });
        }

        public void Trigger(int id)
        {
            DoTrigger(new int[] { id });
        }

        public void SendCancellation(Appointment a)
        {
            if (a.Status != AppointmentStatus.Cancelled)
                return;

            var tempFolder = _configuration["TempFolder"];
            if (Directory.Exists(tempFolder) == false)
                Directory.CreateDirectory(tempFolder);
            var randomName = "notification_cancellation_" + a.Id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var path = Path.Combine(tempFolder, randomName + ".pdf");
            var text = new StringBuilder();

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(path, FileMode.Create));

            doc.AddTitle("Cancelación de turno");
            doc.Open();
            iTextSharp.text.Font _titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _subtitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            var p1 = new Paragraph("VACUNASSIST", _titleFont);
            p1.Alignment = Element.ALIGN_CENTER;
            var p2 = new Paragraph("Notificación de cancelación de turno", _subtitle);
            p2.Alignment = Element.ALIGN_CENTER;
            doc.Add(p1);
            doc.Add(p2);
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("Paciente: " + a.Patient.FullName + " (DNI: " + a.Patient.DNI + ")", _standardFont));
            doc.Add(new Paragraph("Su turno para aplicarse la vacuna de " + a.Vaccine.Name + " con fecha " + a.Date.Value.ToString("dd/MM/yyyy HH:mm:ss") + " ha sido CANCELADO.", _standardFont));
            doc.Add(Chunk.NEWLINE);
            var p3 = new Paragraph("Lo esperamos próximamente", _subtitle);
            p3.Alignment = Element.ALIGN_CENTER;
            doc.Add(p3);
            doc.Close();
            writer.Close();
        }


        private void DoTrigger(int[] ids)
        {
            var threshold = DateTime.Now.AddDays(4); // 3 days from today
            var query = _context.Appointments.Where(x => x.Status == AppointmentStatus.Confirmed
            && x.Date >= DateTime.Now.Date
            && x.Date < threshold
            && x.Notified == false);
            if (ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            var appointments = query.Include(x => x.Patient)
            .Include(x => x.Vaccine)
            .Include(x => x.Vaccinator)
            .ToArray();
            foreach (var a in appointments)
            {
                GenerateNotification(a);
                a.Notified = true;
            }
            _context.SaveChanges();
        }

        private void GenerateNotification(Appointment a)
        {
            if (a.Status != AppointmentStatus.Confirmed)
                return;

            var tempFolder = _configuration["TempFolder"];
            if (Directory.Exists(tempFolder) == false)
                Directory.CreateDirectory(tempFolder);
            var randomName = "notification_" + a.Id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var path = Path.Combine(tempFolder, randomName + ".pdf");
            var text = new StringBuilder();

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(path, FileMode.Create));

            doc.AddTitle("Rercodatorio de turno");
            doc.Open();
            iTextSharp.text.Font _titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _subtitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            var p1 = new Paragraph("VACUNASSIST", _titleFont);
            p1.Alignment = Element.ALIGN_CENTER;
            var p2 = new Paragraph("Notificación de vacunación", _subtitle);
            p2.Alignment = Element.ALIGN_CENTER;
            doc.Add(p1);
            doc.Add(p2);
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("Paciente: " + a.Patient.FullName + " (DNI: " + a.Patient.DNI + ")", _standardFont));
            doc.Add(new Paragraph("Recuerde que tiene un turno pendiente para aplicarse la vacuna de " + a.Vaccine.Name, _standardFont));
            doc.Add(new Paragraph("Fecha/Hora de turno: " + a.Date.Value.ToString("dd/MM/yyyy HH:mm:ss"), _standardFont));
            doc.Add(new Paragraph("Sede: " + a.PreferedOffice.Name + " (" + a.PreferedOffice.Address + ")", _standardFont));
            doc.Add(new Paragraph("Su vacunador/a será: " + a.Vaccinator.FullName, _standardFont));
            doc.Add(Chunk.NEWLINE);
            var p3 = new Paragraph((a.Patient.Gender == "male" ? "Lo esperamos" : "La esperamos"), _subtitle);
            p3.Alignment = Element.ALIGN_CENTER;
            doc.Add(p3);
            doc.Close();
            writer.Close();
        }
    }
}