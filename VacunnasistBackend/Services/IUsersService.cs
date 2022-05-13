using VacunassistBackend.Models;
using VacunassistBackend.Utils;

namespace VacunassistBackend.Services
{
    public interface IUsersService
    {
        User Authenticate(string userName, string password);
    }

    public class UsersService : IUsersService
    {
        private DataContext _context;

        public UsersService(DataContext context)
        {
            this._context = context;
        }
        public User Authenticate(string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == userName && x.IsActive);
            if (user != null && PasswordHash.ValidatePassword(password, user.PasswordHash))
                return user;

            return null;
        }
    }
}