﻿using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;
using InnoGotchiGame.Domain.BaseModels;
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
        private IValidator<IUser> _validator;
        private IRepositoryManager _repositoryManager;
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<IUser> validator)
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
            var managerResult = new ManagerResult();

            if (!await IsUniqueEmailAsync(user.Email, managerResult))
            {
                return managerResult;
            }

            var dataUser = _mapper.Map<IUser>(user);
            dataUser.PasswordHach = StringToHach(password);

            var validationResult = _validator.Validate(dataUser);
            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            _userRepository.Create(dataUser);
            await _repositoryManager.SaveAsync();
            _repositoryManager.Detach(dataUser);
            user.Id = dataUser.Id;
            return managerResult;
        }

        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="updatedId">IUser id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateDataAsync(int updatedId, UserDTO newUser)
        {
            ManagerResult managerResult = new ManagerResult();
            var dataUser = _mapper.Map<IUser>(newUser);

            if (!await CheckUserIdAsync(updatedId, managerResult))
            {
                return managerResult;
            }

            var oldUser = await _userRepository.GetItems(true).FirstAsync(x => x.Id == updatedId);
            oldUser.Picture = dataUser.Picture;
            oldUser.FirstName = dataUser.FirstName;
            oldUser.LastName = dataUser.LastName;

            await _repositoryManager.SaveAsync();
            _repositoryManager.Detach(dataUser);
            
            return managerResult;
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="updatedId">IUser id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdatePasswordAsync(int updatedId, string oldPassword, string newPassword)
        {
            ManagerResult managerResult = new ManagerResult();
            if (!await CheckUserIdAsync(updatedId, managerResult))
            {
                return managerResult;
            }

            var dataUser = await _userRepository.GetItems(true).FirstAsync(x => x.Id == updatedId);

            if (dataUser.PasswordHach != StringToHach(oldPassword))
            {
                managerResult.Errors.Add("Old and new password are not equal");
                return managerResult;
            }

            dataUser.PasswordHach = StringToHach(newPassword);

            await _repositoryManager.SaveAsync();
            _repositoryManager.Detach(dataUser);

            return managerResult;
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="deletedId">IUser id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int deletedId)
        {
            var managerResult = new ManagerResult();
            if (!await CheckUserIdAsync(deletedId, managerResult))
            {
                return managerResult;
            }

            var user = await _userRepository.GetItems(false).FirstAsync(x => x.Id == deletedId);
            _userRepository.Delete(user);

            return managerResult;
        }

        /// <returns>user with special <paramref name="id"/> </returns>
        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetItems(false).FirstAsync(x => x.Id == userId);
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Searches for a user in the database
        /// </summary>
        /// <returns>Finded user or null</returns>
        public async Task<UserDTO?> FindUserInDbAsync(string email, string password)
        {
            string passwordHach = StringToHach(password);
            var user = await _userRepository.GetItems(false).FirstAsync(x => x.Email == email && x.PasswordHach == passwordHach);
            return _mapper.Map<UserDTO>(user);
        }

        /// <returns>Filtered and sorted list of users</returns>
        public async Task<IEnumerable<UserDTO>> GetUsersAsync(Filtrator<IUser>? filtrator = null, Sorter<IUser>? sorter = null)
        {
            var users = await GetUsersQuary(filtrator, sorter).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> users</returns>
        public async Task<IEnumerable<UserDTO>> GetUsersPageAsync(int pageSize, int pageNumber, Filtrator<IUser>? filtrator = null, Sorter<IUser>? sorter = null)
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

        private async Task<bool> CheckUserIdAsync(int userId, ManagerResult managerResult)
        {
            if (!await _userRepository.IsItemExistAsync(x => x.Id == userId))
            {
                managerResult.Errors.Add("The user ID is not in the database");
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

        private IQueryable<IUser> GetUsersQuary(Filtrator<IUser>? filtrator = null, Sorter<IUser>? sorter = null)
        {
            var users = _userRepository.GetItems(false);
            users = filtrator != null ? filtrator.Filter(users) : users;
            users = sorter != null ? sorter.Sort(users) : users;
            return users;
        }
    }
}
