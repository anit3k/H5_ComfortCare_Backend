using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Data
{   
    public class GetStatementPeriod : IGetStatementPeriod
    {
        private readonly IPlanManager _manager;

        public GetStatementPeriod(IPlanManager manager)
        {
            _manager = manager;
        }
        public List<RouteEntity> CreateStatementPeriod(int days, int numberOfAssignments)
        {
            var result = _manager.CalculateNewStatementPeriod(days, numberOfAssignments);
            return result;
        }

        public List<EmployeeEntity> CreateEmployeeRoutes(int employeeID)
        {
            var result = _manager.GetEmployeeRoutes(employeeID);
            return result;
        }

        public void WipeAllRoutes()
        {
            _manager.WipeAllRoutes();
        }
    }
}
