using ComfortCare.Domain.BusinessLogic;
using System.Data.Common;

namespace ComfortCare.Data
{
    /// <summary>
    /// This repository is used by the planmanager to read information from database and save routes calculated for a 
    /// statement period
    /// </summary>
    public class RouteRepo : IRepo
    {
        #region fields
        private readonly ComfortCareDbContext _context;
        #endregion

        #region Constructor
        public RouteRepo(ComfortCareDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        // TODO: refactor these method to fit what ever the route calculator algorithm needs.
        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
