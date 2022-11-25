namespace InnoGotchiGame.Application.Managers
{
	public class ManagerRezult
	{
		public bool IsComplete => Errors.Count() == 0;
		public IEnumerable<string> Errors;

		public ManagerRezult(params string[] errors) 
		{
			Errors = errors;
		}
	}
}
