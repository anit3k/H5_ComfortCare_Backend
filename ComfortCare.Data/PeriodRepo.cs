using ComfortCare.Data.Interfaces;
using ComfortCare.Domain.BusinessLogic.interfaces;

namespace ComfortCare.Data
{
    public class PeriodRepo : IPeriodRepo
    {
        private readonly IPeriodManager _periodManager;

        public PeriodRepo(IPeriodManager periodManager)
        {
            _periodManager = periodManager;
        }
        public void CreateNewPeriod(int days, int numberOfAssignments)
        {
            _periodManager.CalculateNewPeriod(days, numberOfAssignments);
        }
    }
}
