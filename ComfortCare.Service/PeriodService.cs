using ComfortCare.Data.Interfaces;
using ComfortCare.Service.Interfaces;

namespace ComfortCare.Service
{
    /// <summary>
    /// This class works only as a "re router" to keep the package by layer structure strict
    /// In time it could hold many more methods to for example validate periods etc.
    /// </summary>
    public class PeriodService : IPeriodService
    {
        #region fields
        private readonly IPeriodRepo _manager;
        #endregion

        #region Constructor
        public PeriodService(IPeriodRepo manager)
        {
            _manager = manager;
        }
        #endregion

        #region Methods
        public void CreatePeriod(int days, int numberOfAssignments)
        {
            _manager.CreateNewPeriod(days, numberOfAssignments);
        }      
        #endregion
    }
}
