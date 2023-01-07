using FluentValidation;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Validators
{
    public class PictureValidator : AbstractValidator<Picture>
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
