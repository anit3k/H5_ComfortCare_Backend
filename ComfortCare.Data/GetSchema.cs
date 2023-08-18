using ComfortCare.Domain.Entities;

namespace ComfortCare.Data
{
    /// <summary>
    /// This class is used to get the schema for the active user/employee
    /// </summary>
    public class GetSchema : IGetSchema
    {
        #region fields
        private readonly ComfortCareDbContext _context;
        #endregion

        #region Constructor
        public GetSchema(ComfortCareDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This methods returns the schema for the active user/employee
        /// </summary>
        /// <param name="employee">Current/active user</param>
        /// <returns>A schema that contains the route for the active user</returns>
        /// <exception cref="NotImplementedException"></exception>
        public EmployeeRoute CurrentSchema(Employee employee)
        {
            // TODO: implement logic to get employee schema from database
            throw new NotImplementedException();
        }
        #endregion
    }
}
