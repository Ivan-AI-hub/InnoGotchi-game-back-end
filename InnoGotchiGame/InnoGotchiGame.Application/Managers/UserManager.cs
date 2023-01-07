using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchiGame.Application.Managers
{
    public class UserManager
    {
        private AbstractValidator<User> _validator;
        private IRepository<User> _repository;
        private IMapper _mapper;

        public UserManager(IRepository<User> repository, IMapper mapper, AbstractValidator<User> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public ManagerRezult Add(UserDTO user, string password)
        {
            var dataUser = _mapper.Map<User>(user);
            dataUser.PasswordHach = StringToHach(password);

            var validationRezult = _validator.Validate(dataUser);
            var managerRezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && IsUniqueEmail(user.Email, managerRezult))
            {
                var newId = _repository.Add(dataUser);
                user.Id = newId;
                _repository.Save();
            }
            return managerRezult;
        }

        public ManagerRezult UpdateData(int updatedId, UserDTO newUser)
        {
            ManagerRezult managerRezult = new ManagerRezult();
            var dataUser = _mapper.Map<User>(newUser);

            if (CheckUserId(updatedId, managerRezult))
            {
                var oldUser = _repository.GetItemById(updatedId);
                dataUser.Email = oldUser.Email;
                dataUser.PasswordHach = oldUser.PasswordHach;
                if(oldUser.Picture != null)
                {
                    dataUser.Picture.Id = oldUser.Picture.Id;
                }

                var validationRezult = _validator.Validate(dataUser);
                managerRezult = new ManagerRezult(validationRezult);
                if (managerRezult.IsComplete)
                {
                    _repository.Update(updatedId, dataUser);
                    _repository.Save();
                }
            }
            return managerRezult;
        }

        public ManagerRezult UpdatePassword(int updatedId, string oldPassword, string newPassword)
        {
            ManagerRezult managerRezult = new ManagerRezult();
            if (CheckUserId(updatedId, managerRezult))
            {
                var dataUser = _repository.GetItemById(updatedId);

                var validationRezult = _validator.Validate(dataUser);
                managerRezult = new ManagerRezult(validationRezult);
                if (dataUser.PasswordHach == StringToHach(oldPassword))
                {
                    dataUser.PasswordHach = StringToHach(newPassword);
                }
                else
                {
                    managerRezult.Errors.Add("Old and new password are not equal");
                }


                if (managerRezult.IsComplete)
                {
                    _repository.Update(updatedId, dataUser);
                    _repository.Save();
                }
            }
            return managerRezult;
        }

        public ManagerRezult Delete(int deletedId)
        {
            var managerRez = new ManagerRezult();
            if (CheckUserId(deletedId, managerRez))
            {
                _repository.Delete(deletedId);
            }

            return managerRez;
        }

        public UserDTO? GetUserById(int userId)
        {
            var user = _repository.GetItemById(userId);
            if (user != null)
                return _mapper.Map<UserDTO>(user);
            else
                return null;
        }

        public UserDTO? FindUserInDb(string email, string password)
        {
            string passwordHach = StringToHach(password);
            var user = _repository.GetItem(x => x.Email == email && x.PasswordHach == passwordHach);
            if (user != null)
                return _mapper.Map<UserDTO>(user);
            else
                return null;
        }

        public IEnumerable<UserDTO> GetUsers(Filtrator<User>? filtrator = null, Sorter<User>? sorter = null)
        {
            var users = GetUsersQuary(filtrator, sorter);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public IEnumerable<UserDTO> GetUsersPage(int pageSize, int pageNumber, Filtrator<User>? filtrator = null, Sorter<User>? sorter = null)
        {
            var users = GetUsersQuary(filtrator, sorter);
            users = users.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        private string StringToHach(string password)
        {
            using (var hashAlg = MD5.Create())
            {
                byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("X2"));
                }
                return builder.ToString();
            }
        }

        private bool CheckUserId(int userId, ManagerRezult rezult)
        {
            if (!_repository.IsItemExist(userId))
            {
                rezult.Errors.Add("The user ID is not in the database");
                return false;
            }
            return true;
        }

        private bool IsUniqueEmail(string email, ManagerRezult managerRezult)
        {
            var quary = GetUsersQuary();
            if (quary.Any(x => x.Email == email))
            {
                managerRezult.Errors.Add("A user with the same Email already exists in the database");
                return false;
            }
            return true;
        }

        private IQueryable<User> GetUsersQuary(Filtrator<User>? filtrator = null, Sorter<User>? sorter = null)
        {
            var users = _repository.GetItems();
            users = filtrator != null ? filtrator.Filter(users) : users;
            users = sorter != null ? sorter.Sort(users) : users;
            return users;
        }
    }
}
