using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    /// <summary>
    /// This interface is used by the plan-manager to get data from the datasource
    /// </summary>
    public interface IRouteConstructionRepo
    {
        public List<Assignment> GetAssignmentsInPeriod(DateTime start, DateTime end);
        public List<Distance> GetDistanceses(List<Assignment> assignmentsForPeriod);

    }
}
