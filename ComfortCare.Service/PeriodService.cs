using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Service.Interfaces;

namespace ComfortCare.Service
{
    public class PeriodService : IPeriodService
    {
        private readonly IPeriodManager _manager;

        public PeriodService(IPeriodManager manager)
        {
            _manager = manager;
        }
        public void CreatePeriod(int days, int numberOfAssignments)
        {
            _manager.CalculateNewPeriod(days, numberOfAssignments);
        }      
    }
}
