using ComfortCare.Data.Interfaces;
using ComfortCare.Service.Interfaces;
using ComfortCare.Service.Models;

namespace ComfortCare.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userService)
        {
            _userRepo = userService;
        }
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
                    Address = routeAssignment.Assignment.Citizen.Residence.CitizenResidence
                }).ToList();

            var employeeSchema = new EmployeeSchemaModel
            {
                Name = employee.EmployeeName,
                Assignments = assignmentsData
            };

            return employeeSchema;
        }

        public bool ValidateUser(string userName, string password)
        {
            return _userRepo.ValidateUserExist(userName, password);
        }
    }
}
