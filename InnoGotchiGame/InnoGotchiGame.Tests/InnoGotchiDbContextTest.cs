using InnoGotchiGame.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Tests
{
    public class InnoGotchiDbContextTest
    {
        string _testConString = "Server=(localdb)\\mssqllocaldb;Database=InnoGotchiTest;Trusted_Connection=True;MultipleActiveResultSets=True;";
        [Fact]
        public void CreateDb()
        {
            var optionsBuilder = new DbContextOptionsBuilder<InnoGotchiGameContext>();
            var options = optionsBuilder.UseSqlServer(_testConString).Options;
            var context = new InnoGotchiGameContext(options);
        }
    }
}