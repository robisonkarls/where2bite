using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Domain.AggregatesModel
{
    public class InspectionStatusTests
    {
        [Fact]
        public void ShouldConvertPassStatus()
        {
            const string statusName = "Pass";

            var actual = InspectionStatus.FromName(statusName);
            
            Assert.Equal(InspectionStatus.Pass, actual);
        }
        [Fact]
        public void ShouldConvertConditionalPassStatus()
        {
            const string statusName = "Conditional Pass";

            var actual = InspectionStatus.FromName(statusName);
            
            Assert.Equal(InspectionStatus.ConditionalPass, actual);
        }
    }
}