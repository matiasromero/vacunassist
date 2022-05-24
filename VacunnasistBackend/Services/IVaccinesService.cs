using VacunassistBackend.Entities;

namespace VacunassistBackend.Services
{
    public interface IVaccinesService
    {
        Vaccine[] GetAll();
    }

    public class VaccinesService : IVaccinesService
    {
        private DataContext _context;

        public VaccinesService(DataContext context)
        {
            this._context = context;
        }

        public Vaccine[] GetAll()
        {
            return _context.Vaccines.Where(x => x.IsActive).ToArray();
        }
    }
}