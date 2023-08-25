using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;

namespace ComfortCare.Data
{
    /// <summary>
    /// This class is used to get the schema for the active user/employee
    /// </summary>
    public class Schema : ISchema
    {
        #region fields
        private readonly ComfortCareRepository _repository;
        #endregion

        #region Constructor
        public Schema(ComfortCareRepository repo)
        {
            _repository = repo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This methods returns the schema for the active user/employee
        /// </summary>
        /// <param name="employee">Current/active user</param>
        /// <returns>A schema that contains the route for the active user</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Tuple<string, List<Tuple<Assignment, DateTime>>> CurrentSchema(string userInitials, string userPassword)
        {
            try
            {
               return _repository.GetSchemas(userInitials, userPassword);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
            
        }
        #endregion
    }
}
