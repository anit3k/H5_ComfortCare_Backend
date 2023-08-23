using ComfortCare.Domain.Entities;

namespace ComfortCare.Data.Interfaces
{
    public interface IStatementPeriod
    {
        public List<RouteEntity> CreateStatementPeriod(int days, int numberOfAssignments);

        public List<EmployeeEntity> CreateEmployeeRoutes(int employeeID);

        public void WipeAllRoutes();
    }
}
