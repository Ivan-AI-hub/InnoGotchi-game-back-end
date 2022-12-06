using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

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

		public ManagerRezult Add(UserDTO user)
		{
			var dataUser = _mapper.Map<User>(user);
			dataUser.PasswordHach = StringToHach(user.Password);

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

				var validationRezult = _validator.Validate(dataUser);
				managerRezult = new ManagerRezult(validationRezult);
				if (validationRezult.IsValid)
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
                if(dataUser.PasswordHach == StringToHach(oldPassword))
				{ 
                    dataUser.PasswordHach = StringToHach(newPassword);
                }
				else
				{
					managerRezult.Errors.Add("Old and new password are not equal");
				}

                var validationRezult = _validator.Validate(dataUser);
                managerRezult = new ManagerRezult(validationRezult);
                if (validationRezult.IsValid)
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
			int passwordHach = StringToHach(password);
			var user = _repository.GetItem(x => x.Email == email && x.PasswordHach == passwordHach);
			if (user != null)
				return _mapper.Map<UserDTO>(user);
			else
				return null;
		}

		public IEnumerable<UserDTO> GetUsers(UserFiltrator? filtrator = null, UserSorter? sorter = null)
		{
			var users = GetUsersQuary(filtrator, sorter);
			return QueryableUserToUserDTO(users);
		}

		public IEnumerable<UserDTO> GetUsersPage(int pageSize, int pageNumber, UserFiltrator? filtrator = null, UserSorter? sorter = null)
		{
			var users = GetUsersQuary(filtrator, sorter);
			users = users.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
			return QueryableUserToUserDTO(users);
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

		private IQueryable<User> GetUsersQuary(UserFiltrator? filtrator = null, UserSorter? sorter = null)
		{
			var users = _repository.GetItems();
			users = filtrator != null ? filtrator.Filter(users) : users;
			users = sorter != null ? sorter.Sort(users) : users;
			return users;
		}

		private IEnumerable<UserDTO> QueryableUserToUserDTO(IQueryable<User> users)
		{
			var DTOUsers = new List<UserDTO>();
			foreach (var user in users)
				DTOUsers.Add(_mapper.Map<UserDTO>(user));
			return DTOUsers;
		}
	}
}
