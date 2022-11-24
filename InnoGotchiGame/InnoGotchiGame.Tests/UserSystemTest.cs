using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InnoGotchiGame.Tests
{
    public class UserSystemTest
    {
		private readonly DbContextOptions<InnoGotchiGameContext> _contextOptions;

		public UserSystemTest() 
		{
			_contextOptions = new DbContextOptionsBuilder<InnoGotchiGameContext>()
							.UseInMemoryDatabase("BloggingControllerTest")
							.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
							.Options;
		}
		[Fact]
        public void CreateDb()
        {
			var context = new InnoGotchiGameContext(_contextOptions);
        }
    }
}