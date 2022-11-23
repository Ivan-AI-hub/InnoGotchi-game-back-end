namespace InnoGotchiGame.Application.Interfaces
{
	public abstract class Service
	{
		protected IInnoGotchiGameContext Context { get; set; }
		public Service(IInnoGotchiGameContext context)
		{
			Context = context;
		}
	}
}
