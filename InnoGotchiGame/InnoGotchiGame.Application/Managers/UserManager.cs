﻿using AutoMapper;
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
        private IValidator<User> _validator;
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<User> _userRepository;
        private IMapper _mapper;

        public UserManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<User> validator)
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
        public async Task<ManagerResult> AddAsync(UserDTO user, string password)
        {
            var dataUser = _mapper.Map<User>(user);
            dataUser.PasswordHach = StringToHach(password);

            var validationResult = _validator.Validate(dataUser);
            var managerResult = new ManagerResult(validationResult);
            if (validationResult.IsValid && await IsUniqueEmailAsync(user.Email, managerResult))
            {
                _userRepository.Create(dataUser);
                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(dataUser);
                user.Id = dataUser.Id;
            }
            return managerResult;
        }

        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="updatedId">User id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateDataAsync(int updatedId, UserDTO newUser)
        {
            ManagerResult managerResult = new ManagerResult();
            var dataUser = _mapper.Map<User>(newUser);

            if (await CheckUserIdAsync(updatedId, managerResult))
            {
                var oldUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == updatedId, false);
                oldUser.Picture = dataUser.Picture;
                oldUser.FirstName = dataUser.FirstName;
                oldUser.LastName = dataUser.LastName;

                var validationResult = _validator.Validate(oldUser);
                managerResult = new ManagerResult(validationResult);
                if (managerResult.IsComplete)
                {
                    _repositoryManager.User.Update(oldUser);
                    await _repositoryManager.SaveAsync();
                    _repositoryManager.Detach(dataUser);
                }
            }
            return managerResult;
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="updatedId">User id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdatePasswordAsync(int updatedId, string oldPassword, string newPassword)
        {
            ManagerResult managerResult = new ManagerResult();
            if (await CheckUserIdAsync(updatedId, managerResult))
            {
                var dataUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == updatedId, false);

                var validationResult = _validator.Validate(dataUser);
                managerResult = new ManagerResult(validationResult);
                if (dataUser.PasswordHach == StringToHach(oldPassword))
                {
                    dataUser.PasswordHach = StringToHach(newPassword);
                    _repositoryManager.User.Update(dataUser);
                    await _repositoryManager.SaveAsync();
                    _repositoryManager.Detach(dataUser);
                }
                else
                {
                    managerResult.Errors.Add("Old and new password are not equal");
                }
            }
            return managerResult;
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="deletedId">User id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int deletedId)
        {
            var managerRez = new ManagerResult();
            if (await CheckUserIdAsync(deletedId, managerRez))
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == deletedId, false);
                _userRepository.Delete(user);
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

        private async Task<bool> CheckUserIdAsync(int userId, ManagerResult result)
        {
            if (!await _userRepository.IsItemExistAsync(x => x.Id == userId))
            {
                result.Errors.Add("The user ID is not in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> IsUniqueEmailAsync(string email, ManagerResult managerResult)
        {
            if (await _userRepository.IsItemExistAsync(x => x.Email == email))
            {
                managerResult.Errors.Add("A user with the same Email already exists in the database");
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
