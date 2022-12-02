using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;


namespace InnoGotchiGame.Application.Managers
{
	public class PetManager
	{
		private AbstractValidator<Pet> _validator;
		private IRepository<Pet> _repository;
		private IMapper _mapper;

		public PetManager(IRepository<Pet> repository, IMapper mapper, AbstractValidator<Pet> validator)
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
			return new ManagerRezult(validationRezult);
		}

		public ManagerRezult Update(int id, PetDTO pet) 
		{
			
			var managerRez = new ManagerRezult();
			if (CheckPetId(id, managerRez))
			{
				var dataPet = _mapper.Map<Pet>(pet);
				var validationRezult = _validator.Validate(dataPet);
				if (validationRezult.IsValid)
				{
					_repository.Update(id, dataPet);
					_repository.Save();
				}
				managerRez = new ManagerRezult(validationRezult);
			}
			return managerRez;
		}

		public ManagerRezult Delete(int id)
		{
			var managerRez = new ManagerRezult();
			if (CheckPetId(id, managerRez))
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

		private bool CheckPetId(int id, ManagerRezult rezult)
		{
			if (_repository.IsItemExist(id))
			{
				rezult.Errors.Add("The pet ID is not in the database");
				return false;
			}
			return true;
		}
	}
}
