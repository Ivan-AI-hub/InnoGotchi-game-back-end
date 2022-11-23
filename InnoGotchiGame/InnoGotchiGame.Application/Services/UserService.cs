using FluentValidation.Results;
using InnoGotchiGame.Application.Interfaces;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace InnoGotchiGame.Application.Services
{
	public class UserService : Service
	{
		private UserValidator _validator;
		public UserService(IInnoGotchiGameContext context) : base(context)
		{
			_validator = new UserValidator();
		}

		public User? GetUserById(int id)
		{
			return Context.Users.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<User> GetUsers(Func<User, bool>? whereRule = null,
										 Func<User, dynamic>? orderByRule = null,
										 bool isDescendingOrder = false) 
		{
			var users = Context.Users
				.Include(x => x.OwnPetFarm)
				.Include(x => x.SentFriendships)
				.Include(x => x.AcceptedFriendships);

			if (whereRule != null)
				users.Where(whereRule);

			if(orderByRule!= null)
			{
				if (isDescendingOrder)
					users.OrderByDescending(orderByRule);
				else
					users.OrderBy(orderByRule);
			}


			return users;
		}

		public IQueryable<User> GetUsersPage(int pageSize, int pageNumber, 
										 Func<User, bool>? whereRule = null,
										 Func<User, dynamic>? orderByRule = null,
										 bool isDescendingOrder = false) 
		{ 
			return GetUsers(whereRule, orderByRule, isDescendingOrder)
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize);
		}

		public ValidationResult Add(User user)
		{
			var validationRezult = _validator.Validate(user);
			if (validationRezult.IsValid)
			{
				Context.Users.Add(user);
				Context.SaveChanges();
			}

			return validationRezult;
		}

		public ValidationResult Update(int updatedId, User user)
		{
			var validationRezult = _validator.Validate(user);
			if (validationRezult.IsValid)
			{
				user.Id= updatedId;
				Context.Users.Update(user);
				Context.SaveChanges();
			}

			return validationRezult;
		}

		public void Delete(int id)
		{
			var user = GetUserById(id);
			if (user != null)
			{
				Context.Users.Remove(user);
				Context.SaveChanges();
			}
		}
	}
}
