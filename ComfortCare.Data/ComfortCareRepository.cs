using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Data
{
    /// <summary>
    /// This repository is used by the planmanager to read information from database and save routes calculated for a 
    /// statement period
    /// </summary>
    public class ComfortCareRepository : IRouteConstructionRepo, IEmployeesRepo
    {
        #region fields
        private readonly ComfortCareDbContext _context;
        #endregion

        #region Constructor
        public ComfortCareRepository(ComfortCareDbContext context)
        {
            _context = context;
        }      
        #endregion

        #region Methods
        // TODO: refactor these method to fit what ever the route calculator algorithm needs.
        public List<AssignmentEntity> GetAssignmentsInPeriod(DateTime start, DateTime end)
        {            
            throw new NotImplementedException();
        }

        public List<DistanceEntity> GetDistanceses(List<AssignmentEntity> assignmentsForPeriod)
        {
            throw new NotImplementedException();
        }
        public List<IEmployeesRepo> GetAllEmployees()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
