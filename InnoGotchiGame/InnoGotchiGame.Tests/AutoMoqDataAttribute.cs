using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
