using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Entities;
using VacunassistBackend.Infrastructure;
using VacunassistBackend.Models;
using VacunassistBackend.Utils;

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

        void AddVaccine(int id, AddVaccineRequest model);
        void Update(int id, UpdateUserRequest model);
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
            return _context.Users.Include(u => u.Vaccines).ThenInclude(v => v.Vaccine).FirstOrDefault(x => x.Id == id);
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
            return _context.Users.Include(u => u.Vaccines).ToArray();
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

        public void Update(int id, UpdateUserRequest model)
        {
            var user = _context.Users.Include(x => x.PreferedOffice).FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new HttpResponseException(400, message: "Usuario no encontrado");

            if (string.IsNullOrEmpty(model.UserName) == false && model.UserName != user.UserName)
            {
                var existOther = _context.Users.Any(x => x.UserName == model.UserName && x.Id != id);
                if (existOther)
                {
                    throw new HttpResponseException(400, message: "Nombre de usuario '" + model.UserName + "' en uso");
                }
                user.UserName = model.UserName;

            }
            if (string.IsNullOrEmpty(model.Password) == false)
            {
                user.PasswordHash = PasswordHash.CreateHash(model.Password);
            }
            if (string.IsNullOrEmpty(model.FullName) == false && model.FullName != user.FullName)
            {
                user.FullName = model.FullName;
            }
            if (string.IsNullOrEmpty(model.DNI) == false && model.DNI != user.DNI)
            {
                user.DNI = model.DNI;
            }
            if (string.IsNullOrEmpty(model.Email) == false && model.Email != user.Email)
            {
                user.Email = model.Email;
            }
            if (string.IsNullOrEmpty(model.PhoneNumber) == false && model.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = model.PhoneNumber;
            }
            if (string.IsNullOrEmpty(model.Gender) == false && model.Gender != user.Gender)
            {
                user.Gender = model.Gender;
            }
            if (model.BelongsToRiskGroup.HasValue && model.BelongsToRiskGroup != user.BelongsToRiskGroup)
            {
                user.BelongsToRiskGroup = model.BelongsToRiskGroup.Value;
            }
            if (model.BirthDate.HasValue && model.BirthDate != user.BirthDate)
            {
                user.BirthDate = model.BirthDate.Value;
            }
            if (model.PreferedOfficeId.HasValue && (user.PreferedOffice == null || user.PreferedOfficeId != model.PreferedOfficeId))
            {
                user.PreferedOfficeId = model.PreferedOfficeId;
            }

            _context.SaveChanges();
        }

        public void AddVaccine(int id, AddVaccineRequest model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("Usuario no encontrado");

            var newVaccine = new AppliedVaccine();
            newVaccine.AppliedDate = model.AppliedDate;
            newVaccine.VaccineId = model.VaccineId;
            newVaccine.Comment = model.Comment;
            user.Vaccines.Add(newVaccine);
            _context.SaveChanges();
        }
    }
}