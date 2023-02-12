using FluentValidation.Results;

namespace InnoGotchiGame.Application.Managers
{
    public class ManagerResult
    {
        public bool IsComplete => Errors.Count() == 0;
        public List<string> Errors { get; }

        public ManagerResult(params string[] errors)
        {
            Errors = errors.ToList();
        }

        public ManagerResult(ValidationResult validationResult)
        {
            Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }
    }
}
