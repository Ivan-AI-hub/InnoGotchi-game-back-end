using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using System.Data;

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

		public ManagerRezult Add(int farmId, PetDTO pet)
		{
			var dataPet = _mapper.Map<Pet>(pet);

			dataPet.FarmId = farmId;
			dataPet.Statistic.BornDate = DateTime.UtcNow;
			dataPet.Statistic.IsAlive = true;
			dataPet.Statistic.FirstHappinessDay = DateTime.UtcNow;
			dataPet.Statistic.DateLastFeed = DateTime.UtcNow;
			dataPet.Statistic.DateLastDrink = DateTime.UtcNow;
			dataPet.Statistic.FeedingCount = 1;
			dataPet.Statistic.DrinkingCount = 1;

			var validationRezult = _validator.Validate(dataPet);
			var managerRezult = new ManagerRezult(validationRezult);
			if (validationRezult.IsValid && IsUniqueName(pet.Statistic.Name, managerRezult))
			{
				_repository.Add(dataPet);
				_repository.Save();
			}
			return managerRezult;
		}

		public ManagerRezult Update(int id, PetDTO pet) 
		{
			
			var managerRez = new ManagerRezult();
			if (CheckPetId(id, managerRez))
			{
				var dataPet = _mapper.Map<Pet>(pet);
				var validationRezult = _validator.Validate(dataPet);
				managerRez = new ManagerRezult(validationRezult);
				if (validationRezult.IsValid && IsUniqueName(pet.Statistic.Name, managerRez))
				{
					_repository.Update(id, dataPet);
					_repository.Save();
				}
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
		public IEnumerable<PetDTO> GetPets(PetFiltrator? filtrator = null, PetSorter? sorter = null)
		{
			var pets = GetPetsQuary(filtrator, sorter);
			return QueryablePetToPetDTO(pets);
		}

		public IEnumerable<PetDTO> GetPetsPage(int pageSize, int pageNumber, PetFiltrator? filtrator = null, PetSorter? sorter = null)
		{
			var pets = GetPetsQuary(filtrator, sorter);
			pets = pets.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
			return QueryablePetToPetDTO(pets);
		}

		private IQueryable<Pet> GetPetsQuary(PetFiltrator? filtrator = null, PetSorter? sorter = null)
		{
			var pets = _repository.GetItems();
			pets = filtrator != null ? filtrator.Filter(pets) : pets;
			pets = sorter != null ? sorter.Sort(pets) : pets;
			return pets;
		}

		private IEnumerable<PetDTO> QueryablePetToPetDTO(IQueryable<Pet> pets)
		{
			var DTOPets = new List<PetDTO>();
			foreach (var pet in pets)
				DTOPets.Add(_mapper.Map<PetDTO>(pet));
			return DTOPets;
		}

		private bool IsUniqueName(string name, ManagerRezult managerRezult)
		{
			var quary = GetPetsQuary();
			if (quary.Any(x => x.Statistic.Name == name))
			{
				managerRezult.Errors.Add("A pet with the same Name already exists in the database");
				return false;
			}
			return true;
		}
	}
}
