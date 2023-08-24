using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    /// <summary>
    /// This interface is used by the plan-manager to get data from the datasource
    /// </summary>
    public interface IRouteRepo
    {
        public List<AssignmentEntity> GetNumberOfAssignments(int numberOfAssignments);
        public List<DistanceEntity> GetDistanceses(List<AssignmentEntity> assignmentsForPeriod);

    }
}
