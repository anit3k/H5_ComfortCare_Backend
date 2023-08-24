using ComfortCare.Data.Interfaces;
using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Data
{
    public class Period : IPeriod
    {
        private readonly IPlanManager _manager;

        public Period(IPlanManager manager)
        {
            _manager = manager;
        }
        public void CreatePeriod(int days, int numberOfAssignments)
        {
            _manager.CalculateNewPeriod(days, numberOfAssignments);
        }

        //public List<EmployeeEntity> CreateEmployeeRoutes(int employeeID)
        //{
        //    var result = _manager.GetEmployeeRoutes(employeeID);
        //    return result;
        //}

        //public void WipeAllRoutes()
        //{
        //    _manager.WipeAllRoutes();
        //}
    }
}
