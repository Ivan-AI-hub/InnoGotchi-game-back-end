using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Managers;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a user
    /// </summary>
    public class UserManager
    {
        private AbstractValidator<User> _validator;
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<User> _userRepository;
        private IMapper _mapper;

        public UserManager(IRepositoryManager repositoryManager, IMapper mapper, AbstractValidator<User> validator)
        {
            _repositoryManager = repositoryManager;
            _userRepository = repositoryManager.User;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Adds <paramref name="user"/> to database
        /// </summary>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> AddAsync(UserDTO user, string password)
        {
            var dataUser = _mapper.Map<User>(user);
            dataUser.PasswordHach = StringToHach(password);

            var validationRezult = _validator.Validate(dataUser);
            var managerRezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && await IsUniqueEmailAsync(user.Email, managerRezult))
            {
                _userRepository.Create(dataUser);
                _repositoryManager.SaveAsync().Wait();
                user.Id = dataUser.Id;
            }
            return managerRezult;
        }

        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="updatedId">User id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> UpdateDataAsync(int updatedId, UserDTO newUser)
        {
            ManagerRezult managerRezult = new ManagerRezult();
            var dataUser = _mapper.Map<User>(newUser);

            if (await CheckUserIdAsync(updatedId, managerRezult))
            {
                var oldUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == updatedId, true);
                oldUser.Picture = dataUser.Picture;
                oldUser.FirstName = dataUser.FirstName;
                oldUser.LastName = dataUser.LastName;

                var validationRezult = _validator.Validate(oldUser);
                managerRezult = new ManagerRezult(validationRezult);
                if (managerRezult.IsComplete)
                {
                    _repositoryManager.SaveAsync().Wait();
                }
            }
            return managerRezult;
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="updatedId">User id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> UpdatePasswordAsync(int updatedId, string oldPassword, string newPassword)
        {
            ManagerRezult managerRezult = new ManagerRezult();
            if (await CheckUserIdAsync(updatedId, managerRezult))
            {
                var dataUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == updatedId, false);

                var validationRezult = _validator.Validate(dataUser);
                managerRezult = new ManagerRezult(validationRezult);
                if (dataUser.PasswordHach == StringToHach(oldPassword))
                {
                    dataUser.PasswordHach = StringToHach(newPassword);
                    _repositoryManager.SaveAsync().Wait();
                }
                else
                {
                    managerRezult.Errors.Add("Old and new password are not equal");
                }
            }
            return managerRezult;
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="deletedId">User id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> DeleteAsync(int deletedId)
        {
            var managerRez = new ManagerRezult();
            if (await CheckUserIdAsync(deletedId, managerRez))
            {
                _userRepository.Delete(await _userRepository.FirstOrDefaultAsync(x => x.Id == deletedId, false));
            }

            return managerRez;
        }

        /// <returns>user with special <paramref name="id"/> </returns>
        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId, false);
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Searches for a user in the database
        /// </summary>
        /// <returns>Finded user or null</returns>
        public async Task<UserDTO?> FindUserInDbAsync(string email, string password)
        {
            string passwordHach = StringToHach(password);
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == email && x.PasswordHach == passwordHach, false);
            return _mapper.Map<UserDTO>(user);
        }

        /// <returns>Filtered and sorted list of users</returns>
        public async Task<IEnumerable<UserDTO>> GetUsersAsync(Filtrator<User>? filtrator = null, Sorter<User>? sorter = null)
        {
            var users = await GetUsersQuary(filtrator, sorter).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> users</returns>
        public async Task<IEnumerable<UserDTO>> GetUsersPageAsync(int pageSize, int pageNumber, Filtrator<User>? filtrator = null, Sorter<User>? sorter = null)
        {
            var users = GetUsersQuary(filtrator, sorter);
            users = users.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return _mapper.Map<IEnumerable<UserDTO>>(await users.ToListAsync());
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

        private async Task<bool> CheckUserIdAsync(int userId, ManagerRezult rezult)
        {
            if (!await _userRepository.IsItemExistAsync(x => x.Id == userId))
            {
                rezult.Errors.Add("The user ID is not in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> IsUniqueEmailAsync(string email, ManagerRezult managerRezult)
        {
            if (await _userRepository.IsItemExistAsync(x => x.Email == email))
            {
                managerRezult.Errors.Add("A user with the same Email already exists in the database");
                return false;
            }
            return true;
        }

        private IQueryable<User> GetUsersQuary(Filtrator<User>? filtrator = null, Sorter<User>? sorter = null)
        {
            var users = _userRepository.GetItems(false);
            users = filtrator != null ? filtrator.Filter(users) : users;
            users = sorter != null ? sorter.Sort(users) : users;
            return users;
        }
    }
}
