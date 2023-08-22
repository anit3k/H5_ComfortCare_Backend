using Microsoft.EntityFrameworkCore;

namespace ComfortCare.Data
{
    /// <summary>
    /// This class is used to validate if the current user is alowed to enter the system
    /// </summary>
    public class Validate : IValidate
    {
        #region fields
        private readonly ComfortCareDbContext _context;
        #endregion

        #region Constructor
        public Validate(ComfortCareDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method validate is the user exist, and if the password matches 
        /// </summary>
        /// <param name="initials">The initials of the user trying to login</param>
        /// <param name="password">The password current user is trying to login with</param>
        /// <returns>if password or user is wrong returns false, else true</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ValidateUser(string initials, string password)
        {
            // TODO: implement logic for validation of user
            var employeeMatchingUserInput = _context.Employee.Where(e => e.Initials == initials && e.EmployeePassword == password).ToList();

            if (employeeMatchingUserInput.Count > 0)
            {
                return true;
            } 
            else {
                return false;
            }
        }
        #endregion
    }
}
