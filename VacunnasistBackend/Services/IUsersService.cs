using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Models;
using VacunassistBackend.Utils;
using VacunnasistBackend.Entities;

namespace VacunassistBackend.Services
{
    public interface IUsersService
    {
        User Authenticate(string userName, string password);
        User Get(int id);
        bool Exists(string userName);
        bool Exists(int id);
        bool Register(RegisterRequest model);
        User[] GetAll();

        void Update(int id, string? password);
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

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }


        public bool Exists(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);
        }

        public bool Exists(int id)
        {
            return _context.Users.Any(x => x.Id == id);
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

        public void Update(int id, string? password)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                throw new ApplicationException("Usuario no encontrado");

            if (string.IsNullOrEmpty(password) == false)
            {
                user.PasswordHash = PasswordHash.CreateHash(password);
            }

            _context.SaveChanges();
        }
    }
}