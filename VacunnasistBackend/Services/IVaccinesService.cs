using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Services
{
    public interface IVaccinesService
    {
        Vaccine[] GetAll();
        bool ExistsApplied(int id);
        AppliedVaccine GetApplied(int id);
    }

    public class VaccinesService : IVaccinesService
    {
        private DataContext _context;

        public VaccinesService(DataContext context)
        {
            this._context = context;
        }

        public bool ExistsApplied(int id)
        {
            return _context.AppliedVaccines.Any(x => x.Id == id);
        }

        public Vaccine[] GetAll()
        {
            return _context.Vaccines.Where(x => x.IsActive).ToArray();
        }

        public AppliedVaccine GetApplied(int id)
        {
            return _context.AppliedVaccines.Include(u => u.Vaccine).First(x => x.Id == id);
        }
    }
}