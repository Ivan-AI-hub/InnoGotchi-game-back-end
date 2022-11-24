using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Mappings;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

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


			Assert.True(rez.IsValid);
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


			Assert.False(rez.IsValid);
		}
	}
}