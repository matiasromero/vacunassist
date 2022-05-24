using VacunassistBackend.Entities;

namespace VacunassistBackend.Services
{
    public interface IOfficesService
    {
        Office[] GetAll();
    }

    public class OfficesService : IOfficesService
    {
        private DataContext _context;

        public OfficesService(DataContext context)
        {
            this._context = context;
        }

        public Office[] GetAll()
        {
            return _context.Offices.Where(u => u.IsActive).ToArray();
        }
    }
}