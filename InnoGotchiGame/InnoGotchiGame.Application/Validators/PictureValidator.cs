using FluentValidation;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;

namespace InnoGotchiGame.Application.Validators
{
    public class PictureValidator : AbstractValidator<IPicture>
    {
        public PictureValidator()
        {
            RuleFor(picture => picture.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(picture => picture.Image)
                .NotEmpty()
                .NotNull();
            RuleFor(picture => picture.Description)
                .NotEmpty()
                .NotNull();
            RuleFor(picture => picture.Format)
                .NotEmpty()
                .NotNull();
        }
    }
}
