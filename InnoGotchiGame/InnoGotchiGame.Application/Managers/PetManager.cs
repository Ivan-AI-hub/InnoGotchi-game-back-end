using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchiGame.Application.Managers
{
	public class PetManager
	{
		private AbstractValidator<Pet> _validator;
		private IRepository<Pet> _repository;
		private IMapper _mapper;

		public PetManager(AbstractValidator<Pet> validator, IRepository<Pet> repository, IMapper mapper)
		{
			_validator = validator;
			_repository = repository;
			_mapper = mapper;
		}

		public ManagerRezult Add(PetDTO pet)
		{
			var dataPet = _mapper.Map<Pet>(pet);
			var validationRezult = _validator.Validate(dataPet);
			if(validationRezult.IsValid)
			{
				_repository.Add(dataPet);
				_repository.Save();
			}
			return _mapper.Map<ManagerRezult>(validationRezult);
		}

		public ManagerRezult Update(int id, PetDTO pet) 
		{
			var dataPet = _mapper.Map<Pet>(pet);
			var managerRez = new ManagerRezult();
			CheckPetId(id, managerRez);
			if (managerRez.IsComplete)
			{
				var validationRezult = _validator.Validate(dataPet);
				if (validationRezult.IsValid)
				{
					_repository.Update(id, dataPet);
					_repository.Save();
				}
				managerRez = _mapper.Map<ManagerRezult>(validationRezult);
			}
			return managerRez;
		}

		public ManagerRezult Delete(int id)
		{
			var managerRez = new ManagerRezult();
			CheckPetId(id, managerRez);
			if (managerRez.IsComplete)
			{
				_repository.Delete(id);
			}
			return managerRez;
		}

		public PetDTO? GetPetById(int id)
		{
			var petData = _repository.GetItemById(id);
			var pet = _mapper.Map<PetDTO>(petData);
			return pet;
		}

		private Pet? CheckPetId(int id, ManagerRezult rezult)
		{
			var pet = _repository.GetItemById(id);
			if (pet == null)
				rezult.Errors.Add("The pet ID is not in the database");
			return pet;
		}
	}
}
