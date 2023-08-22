using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    public interface IPlanManager
    {
        public List<RouteEntity> CalculateNewStatementPeriod(int numberOfDays, int numbersOfAssignments);
    }
}