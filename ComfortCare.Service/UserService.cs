using ComfortCare.Data.Interfaces;
using ComfortCare.Service.Interfaces;
using ComfortCare.Service.Models;

namespace ComfortCare.Service
{
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
        public EmployeeSchemaModel GetEmployeeSchema(string userName, string password)
        {
            var employee = _userRepo.GetUsersWorkSchedule(userName, password);

            var assignmentData = new List<AssignmentSchemaModel>();
            foreach (var route in employee)
            {
                foreach (var assignment in route.Assignments)
                {
                    var temp = new AssignmentSchemaModel();
                    temp.Title = assignment.Title;
                    temp.Description = assignment.Description;
                    temp.CitizenName = assignment.CitizenName;
                    temp.Address = assignment.Address;
                    temp.StartDate = assignment.StartDate;
                    temp.EndDate = assignment.EndDate;
                    assignmentData.Add(temp);
                }
            }
            
            var employeeSchema = new EmployeeSchemaModel
            {
                Name = employee[0].Name,
                Assignments = assignmentData
            };

            return employeeSchema;
        }

        public bool ValidateUser(string userName, string password)
        {
            return _userRepo.ValidateUserExist(userName, password);
        }
        #endregion
    }
}
