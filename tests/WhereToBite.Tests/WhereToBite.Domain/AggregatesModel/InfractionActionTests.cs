using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Domain.AggregatesModel
{
    public class InfractionActionTests
    {
        [Fact]
        public void ShouldConvertNoticeToComply()
        {
            const string statusName = "Notice to Comply";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.NoticeToComply, actual);
        }
        
        [Fact]
        public void ShouldConvertEducationProvided()
        {
            const string statusName = "Education Provided";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.EducationProvided, actual);
        }
        
        [Fact]
        public void ShouldConvertCorrectedDuringInspection()
        {
            const string statusName = "Corrected During Inspection";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.CorrectedDuringInspection, actual);
        }
        
        [Fact]
        public void ShouldConvertSummons()
        {
            const string statusName = "Summons";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.Summons, actual);
        }
        
        [Fact]
        public void ShouldConvertSummonsAndHealthHazardOrder()
        {
            const string statusName = "Summons and Health Hazard Order";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.SummonsAndHealthHazardOrder, actual);
        }
        
        [Fact]
        public void ShouldConvertTicket()
        {
            const string statusName = "Ticket";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.Ticket, actual);
        }
        
        [Fact]
        public void ShouldConvertNotInCompliance()
        {
            const string statusName = "Not in Compliance";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.NotInCompliance, actual);
        }
        
        [Fact]
        public void ShouldConvertSummonsByLaw()
        {
            const string statusName = "Summons-ByLaw";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.SummonsByLaw, actual);
        }
        
        [Fact]
        public void ShouldConvertClosureOrder()
        {
            const string statusName = "Closure Order";

            var actual = InfractionAction.FromName(statusName);
            
            Assert.Equal(InfractionAction.ClosureOrder, actual);
        }
    }
}