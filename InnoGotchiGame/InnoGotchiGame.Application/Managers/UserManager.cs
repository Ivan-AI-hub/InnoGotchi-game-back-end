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
			if (validationRezult.IsValid)
			{
				var newId = _repository.Add(dataUser);
				user.Id = newId;
				_repository.Save();
			}
			return _mapper.Map<ManagerRezult>(validationRezult);
		}
		public void SendCollaborationInvite(UserDTO sender, UserDTO recepient)
		{
			var invite = new ColaborationRequestDTO() {RequestSender = sender, RequestReceiver = recepient, Status = ColaborationRequestStatusDTO.Undefined };
			sender.SentColaborations.Add(invite);
			recepient.AcceptedColaborations.Add(invite);
			Update(sender.Id, sender);
			Update(recepient.Id, recepient);
		}

		public ManagerRezult Update(int updatedId, UserDTO newUser)
		{
			ManagerRezult rezult = new ManagerRezult();
			var dataUser = _mapper.Map<User>(newUser);

			if (CheckUserId(updatedId, rezult))
			{
				if (newUser.Password == String.Empty)
				{
					dataUser.PasswordHach = _repository.GetItemById(updatedId).PasswordHach;
				}
				else
				{
					dataUser.PasswordHach = StringToHach(newUser.Password);
				}

				var validationRezult = _validator.Validate(dataUser);
				if (validationRezult.IsValid)
				{
					_repository.Update(updatedId, dataUser);
					_repository.Save();
				}
				rezult = _mapper.Map<ManagerRezult>(validationRezult);
			}
			return rezult;
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
