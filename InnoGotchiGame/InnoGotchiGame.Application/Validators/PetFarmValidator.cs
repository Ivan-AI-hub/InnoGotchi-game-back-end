using FluentValidation;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;

namespace InnoGotchiGame.Application.Validators
{
    public class PetFarmValidator : AbstractValidator<IPetFarm>
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
