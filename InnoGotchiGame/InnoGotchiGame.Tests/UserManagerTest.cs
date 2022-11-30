using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Mappings;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InnoGotchiGame.Tests
{
	public class UserManagerTest
	{
		private DbContextOptions<InnoGotchiGameContext> _contextOptions;
		private InnoGotchiGameContext _context;
		private IRepository<User> _repository;
		private AbstractValidator<User> _validator;
		private IMapper _mapper;

		public UserManagerTest()
		{
			_contextOptions = new DbContextOptionsBuilder<InnoGotchiGameContext>()
							.UseInMemoryDatabase("UserManagerTest")
							.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
							.Options;

			_context = new InnoGotchiGameContext(_contextOptions);
			_repository = new UserRepository(_context);
			_validator = new UserValidator();
			var mapperConfig = new MapperConfiguration(config => config.AddProfile(new AssemblyMappingProfile()));
			_mapper = new Mapper(mapperConfig);
		}

		[Fact]
		public void Add_Valid_User()
		{
			UserManager manager = new UserManager(_repository, _mapper, _validator);


			var user = new UserDTO()
			{
				FirstName = "First",
				LastName = "Last",
				Email = "test_user@gmail.com",
				Password = "Test_1234"
			};


			var rez = manager.Add(user);


			Assert.True(rez.IsComplete);
		}

		[Fact]
		public void Delete_Valid_User()
		{
			UserManager manager = new UserManager(_repository, _mapper, _validator);


			var user = new UserDTO()
			{
				FirstName = "First",
				LastName = "Last",
				Email = "test_user@gmail.com",
				Password = "Test_1234"
			};


			manager.Add(user);
			var rez = manager.Delete(user.Id);

			Assert.True(rez.IsComplete);
		}

		[Fact]
		public void Add_Unvalid_User()
		{
			UserManager manager = new UserManager(_repository, _mapper, _validator);


			var user = new UserDTO()
			{
				FirstName = "First",
				LastName = "Last",
				Email = "wrongMail",
				Password = "Test_1234"
			};


			var rez = manager.Add(user);


			Assert.False(rez.IsComplete);
		}

		[Fact]
		public void Update_Unvalid_User()
		{
			UserManager manager = new UserManager(_repository, _mapper, _validator);


			var user = new UserDTO()
			{
				FirstName = "FirstUpdate",
				LastName = "LastUpdate",
				Email = "update@gmail.com",
				Password = "Test_1234"
			};
			manager.Add(user);
			user.Email = "wrongEmail";

			var rez = manager.Update(user.Id, user);

			Assert.False(rez.IsComplete);
		}

		[Fact]
		public void Update_non_existent_id()
		{
			UserManager manager = new UserManager(_repository, _mapper, _validator);

			var user = new UserDTO()
			{
				FirstName = "FirstUpdate",
				LastName = "LastUpdate",
				Email = "update@gmail.com",
				Password = "Test_1234"
			};

			var rez = manager.Update(100, user);

			Assert.False(rez.IsComplete);
		}

		[Fact]
		public void Get_users()
		{
			UserManager manager = new UserManager(_repository, _mapper, _validator);
			var users = new List<UserDTO>();
			int usersCount = 4;
			for (int i = 0; i < usersCount; i++)
			{
				var user = new UserDTO()
				{
					FirstName = "Test"+i,
					LastName = "Test" + i,
					Email = $"update{i}@gmail.com",
					Password = "Test_1234"
				};
				users.Add(user);
			}

			users.ForEach(x => manager.Add(x));

			var dataBaseUsers = manager.GetUsers().ToList();

			for (int i = 0; i < users.Count; i++)
			{
				Assert.Equal(users[i].FirstName, dataBaseUsers[i].FirstName);
			}
		}
	}
}