using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using System.Data;

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

		/// <summary>
		/// Adds user in data base
		/// </summary>
		/// <param name="user">User for adding</param>
		/// <returns>The result of data validation.
		/// If the data has been validated successfully
		/// it will be added to the database otherwise not</returns>
		public ManagerRezult Add(UserDTO user)
		{
			var dataUser = _mapper.Map<User>(user);
			dataUser.PasswordHach = StringToHach(user.Password);

			var validationRezult = _validator.Validate(dataUser);
			if(validationRezult.IsValid)
			{
				_repository.Add(dataUser);
				_repository.Save();
			}
			return _mapper.Map<ManagerRezult>(validationRezult);
		}


		public ManagerRezult Update(int updatedId, UserDTO newUser)
		{
			ManagerRezult rezult = new ManagerRezult();
			var dataUser = _mapper.Map<User>(newUser);

			var oldUser = CheckUserId(updatedId, rezult);
			if (rezult.IsComplete && oldUser != null)
			{
				if (newUser.Password == String.Empty)
				{
					dataUser.PasswordHach = oldUser.PasswordHach;
				}
				else
				{
					dataUser.PasswordHach = StringToHach(newUser.Password);
				}

				var validationRezult = _validator.Validate(dataUser);
				if (validationRezult.IsValid)
				{
					_repository.Add(dataUser);
					_repository.Save();
				}
				rezult = _mapper.Map<ManagerRezult>(validationRezult);
			}
			return rezult;
		}

		/// <summary>
		/// Finds user in data base
		/// </summary>
		/// <param name="service">service for interacting with the database</param>
		/// <param name="email">User email</param>
		/// <param name="password">User password</param>
		/// <returns>Found user or null if the user was not found</returns>
		public User? FindUserInDb(string email, string password)
		{
			int passwordHach = StringToHach(password);
			throw new NotImplementedException();
			//return service.GetUser(x => x.Email == email && x.PasswordHach == passwordHach);
		}

		private int StringToHach(string read)
		{
			int hashedValue = 3074457;
			for (int i = 0; i < read.Length; i++)
			{
				hashedValue += read[i];
				hashedValue *= 3074457;
			}
			return hashedValue;
		}

		private User? CheckUserId(int userId, ManagerRezult rezult)
		{
			var user = _repository.GetItemById(userId);
			if (user == null)
			{
				rezult.Errors.Add("The user ID is not in the database");
			}
			return user;
		}
	}
}
