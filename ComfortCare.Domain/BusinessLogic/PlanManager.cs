namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This class is the manger for creating the statements plan for all the citizens
    /// </summary>
    public class PlanManager : IPlanManager
    {
        #region fields
        private readonly IRepo _repo;
        private readonly IDistance _distance;
        #endregion

        #region Constructor
        public PlanManager(IRepo repo, IDistance distance)
        {
            _repo = repo;
            _distance = distance;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Main method for calculation of the statement period.
        /// Saves the generated schedule to the data source
        /// </summary>
        /// <param name="startDate">The date of which the period begins</param>
        /// <param name="endDate">The date of which the period ends</param>
        public void CalculateNewStatementPeriod(DateTime startDate, DateTime endDate)
        {
            // this is where the route calculater should start
        }       
        #endregion
    }
}
