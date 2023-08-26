using ComfortCare.Domain.BusinessLogic.interfaces;

namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This class is the manger for creating the statements plan for all the citizens, and schema's for all the employees
    /// uisng the ComfortCare Eco System
    /// </summary>
    public class PeriodManager : IPeriodManager
    {
        #region fields
        private readonly RouteGenerator _routeGen;
        private readonly SchemaGenerator _schemaGen;
        #endregion

        #region Constructor
        public PeriodManager(RouteGenerator routeGen, SchemaGenerator schemaGen)
        {
            _routeGen = routeGen;
            _schemaGen = schemaGen;
        }
        #endregion

        #region Methods
        public void CalculateNewPeriod(int numberOfDays, int numberOfAssignments)
        {
            var routes = _routeGen.CalculateDaylyRoutes(numberOfDays, numberOfAssignments);
            _schemaGen.GenerateSchema(routes);
        }
        #endregion
    }
}
