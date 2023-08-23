using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This class is the manger for creating the statements plan for all the citizens, and schema's for all the employees
    /// uisng the ComfortCare Eco System
    /// </summary>
    public class PlanManager : IPlanManager
    {
        #region fields
        private readonly RouteGenerator _routeGen;
        private readonly SchemaGenerator _schemaGen;
        #endregion

        #region Constructor
        public PlanManager(RouteGenerator routeGen, SchemaGenerator schemaGen)
        {
            _routeGen = routeGen;
            _schemaGen = schemaGen;
        }
        #endregion

        #region Methods
        public List<RouteEntity> CalculateNewStatementPeriod(int numberOfDays, int numberOfAssignments)
        {
            var result = _routeGen.CalculateDaylyRoutes(numberOfDays, numberOfAssignments);
            List<EmployeeEntity> employees = _schemaGen.GenerateSchema(result);
            _schemaGen.SaveSchema(employees);

            return result;
        }

        public List<EmployeeEntity> GetEmployeeRoutes(int employeeID)
        {
            var result = _schemaGen.GetRoutesForCurrentEmployee(employeeID);

            return result;
        }

        public void WipeAllRoutes()
        {
           _schemaGen.WipeAllRoutes();
        }
        #endregion
    }
}
