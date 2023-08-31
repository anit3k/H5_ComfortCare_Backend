using ComfortCare.Domain.BusinessLogic.interfaces;

namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This class is the manger for creating the period plan for all the citizens, and schema's for all the employees
    /// this is the main entry point if the core domain logic
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
        /// <summary>
        /// This is the main method for running the algorithms to create new period and
        /// assign employees to the routes created
        /// </summary>
        /// <param name="numberOfDays">The number of days to calculate routes</param>
        /// <param name="numberOfAssignments">The number of assignments for each day</param>
        public void CalculateNewPeriod(int numberOfDays, int numberOfAssignments)
        {
            var routes = _routeGen.CalculateDailyRoutes(numberOfDays, numberOfAssignments);
            _schemaGen.GenerateSchema(routes);
        }
        #endregion
    }
}
