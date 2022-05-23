using Microsoft.AspNetCore.Mvc;
using VacunassistBackend.Models;
using VacunassistBackend.Utils;
using VacunnasistBackend.Entities;

namespace VacunassistBackend.Services
{
    public interface IUsersService
    {
        User Authenticate(string userName, string password);
        bool Exists(string userName);
        bool Register(RegisterRequest model);
        User[] GetAll();
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

        public bool Exists(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);
        }

        public User[] GetAll()
        {
            return _context.Users.ToArray();
        }

        public bool Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.UserName == model.UserName))
                throw new ApplicationException("Nombre de usuario '" + model.UserName + "' en uso");

            try
            {
                var user = new User(model.UserName)
                {
                    Address = model.Address,
                    BelongsToRiskGroup = model.BelongsToRiskGroup,
                    BirthDate = model.BirthDate,
                    DNI = model.DNI,
                    Email = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    PasswordHash = PasswordHash.CreateHash(model.Password),
                    PhoneNumber = model.PhoneNumber,
                    Role = UserRoles.Patient
                };

                // save user
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}