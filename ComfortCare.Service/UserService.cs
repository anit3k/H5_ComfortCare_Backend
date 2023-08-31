using ComfortCare.Data.Interfaces;
using ComfortCare.Service.Interfaces;
using ComfortCare.Service.Models;

namespace ComfortCare.Service
{
    /// <summary>
    /// This service class works as a mediator between the gateway and the data layer
    /// </summary>
    public class UserService : IUserService
    {
        #region fields
        private readonly IUserRepo _userRepo;
        #endregion

        #region Constructor
        public UserService(IUserRepo userService)
        {
            _userRepo = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method gets the employee db model, with all the attributes needed to
        /// map the data used in the UI
        /// </summary>
        /// <param name="userName">the initials of the employee</param>
        /// <param name="password">the password of the employee</param>
        /// <returns>A EmployeeSchemaModel with the users task for the current period</returns>
        public EmployeeSchemaModel GetEmployeeSchema(string userName, string password)
        {
            var employee = _userRepo.GetUsersWorkSchedule(userName, password);

            var assignmentsData = employee.EmployeeRoute
                .SelectMany(route => route.RouteAssignment)
                .Select(routeAssignment => new AssignmentSchemaModel
                {
                    Title = routeAssignment.Assignment.AssignmentType.Title,
                    AssignmentTypeDescription = routeAssignment.Assignment.AssignmentType.AssignmentTypeDescription,
                    Description = routeAssignment.Assignment.AssignmentType.AssignmentTypeDescription,
                    CitizenName = routeAssignment.Assignment.Citizen.CitizenName,
                    StartDate = routeAssignment.ArrivalTime,
                    EndDate = routeAssignment.ArrivalTime.AddSeconds(Convert.ToDouble(routeAssignment.Assignment.AssignmentType.DurationInSeconds)),
                    Address = routeAssignment.Assignment.Citizen.Residence.CitizenResidence                    
                }).ToList();

            var employeeSchema = new EmployeeSchemaModel
            {
                Name = employee.EmployeeName,
                Assignments = assignmentsData
            };

            return employeeSchema;
        }

        /// <summary>
        ///  This method is used to validate if the user trying to login is
        ///  exist in the database
        /// </summary>
        /// <param name="userName">the initials of the current user</param>
        /// <param name="password">the password of the current user</param>
        /// <returns>return true if the current user exist in the db, returns false if not</returns>
        public bool ValidateUser(string userName, string password)
        {
            return _userRepo.ValidateUserExist(userName, password);
        }
        #endregion
    }
}
