using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Domain.AggregatesModel
{
    public class EstablishmentStatusTests
    {
        [Fact]
        public void ShouldConvertPassStatus()
        {
            const string statusName = "Pass";

            var actual = EstablishmentStatus.FromName(statusName);
            
            Assert.Equal(EstablishmentStatus.Pass, actual);
        }
        
        [Fact]
        public void ShouldConvertConditionalPassStatus()
        {
            const string statusName = "Conditional Pass";

            var actual = EstablishmentStatus.FromName(statusName);
            
            Assert.Equal(EstablishmentStatus.ConditionalPass, actual);
        }
        
        [Fact]
        public void ShouldConvertClosed()
        {
            const string statusName = "Closed";

            var actual = EstablishmentStatus.FromName(statusName);
            
            Assert.Equal(EstablishmentStatus.Closed, actual);
        }
    }
}