using ComfortCare.Domain.BusinessLogic.interfaces;

namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This class is the manger for creating the statements plan for all the citizens, and schema's for all the employees
    /// uisng the ComfortCare Eco System
    /// </summary>
    public class PlanManager : IPlanManager
    {
        #region fields

        #endregion

        #region Constructor
        public PlanManager()
        {
         
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
