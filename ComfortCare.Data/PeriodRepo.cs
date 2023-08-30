using ComfortCare.Data.Interfaces;
using ComfortCare.Domain.BusinessLogic.interfaces;

namespace ComfortCare.Data
{
    /// <summary>
    /// This class is used as a mediator for the domain and data layer
    /// In the current version its only responsibility is to re reoute the incomming request to the period manager in
    /// the domain layer, but in time it could also hold logic to collect information about past and present periods from the db
    /// </summary>
    public class PeriodRepo : IPeriodRepo
    {
        #region fields
        private readonly IPeriodManager _periodManager;
        #endregion

        #region Constructor
        public PeriodRepo(IPeriodManager periodManager)
        {
            _periodManager = periodManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// this methods is used to call the period manager in the domain layer to
        /// create new routes
        /// </summary>
        /// <param name="days">The number of days in the period</param>
        /// <param name="numberOfAssignments">The number of assignments in the requested period</param>
        public void CreateNewPeriod(int days, int numberOfAssignments)
        {
            _periodManager.CalculateNewPeriod(days, numberOfAssignments);
        }
        #endregion
    }
}
