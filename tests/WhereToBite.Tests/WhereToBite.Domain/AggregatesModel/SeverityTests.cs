using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Domain.AggregatesModel
{
    public class SeverityTests
    {
        [Fact]
        public void ShouldConvertSignificationSeverity()
        {
            const string statusName = "S - Significant";

            var actual = Severity.FromName(statusName);
            
            Assert.Equal(Severity.Significant, actual);
        }
        
        [Fact]
        public void ShouldConvertMinorSeverity()
        {
            const string statusName = "M - Minor";

            var actual = Severity.FromName(statusName);
            
            Assert.Equal(Severity.Minor, actual);
        }
        
        [Fact]
        public void ShouldConvertCrucialSeverity()
        {
            const string statusName = "C - Crucial";

            var actual = Severity.FromName(statusName);
            
            Assert.Equal(Severity.Crucial, actual);
        }
        
        [Fact]
        public void ShouldConvertNotApplicableSeverity()
        {
            const string statusName = "NA - Not Applicable";

            var actual = Severity.FromName(statusName);
            
            Assert.Equal(Severity.NotApplicable, actual);
        }
    }
}