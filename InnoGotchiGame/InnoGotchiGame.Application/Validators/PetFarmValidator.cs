using FluentValidation;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Validators
{
	public class PetFarmValidator : AbstractValidator<PetFarm>
	{
		public PetFarmValidator() 
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.CreateDate)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.OwnerId)
				.NotNull()
				.NotEmpty();
		}
	}
}
