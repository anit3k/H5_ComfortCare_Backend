using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;
using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Data
{
    public class ComfortCareRepository : IRouteRepo, IEmployeesRepo, IUserRepo
    {
        #region fields
        private readonly IMongoDbContext _context;
        #endregion

        #region Constructor
        public ComfortCareRepository(IMongoDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public List<AssignmentEntity> GetNumberOfAssignments(int assignments)
        {
            var result = _context.GetAll<AssignmentDbModel>("AssignmentCollection");
            List<AssignmentEntity> assignmentEntities = new List<AssignmentEntity>();

            for (int i = 0; i < assignments; i++)
            {
                var temp = new AssignmentEntity()
                {
                    Duration = result[i].DurationInSeconds,
                    Id = result[i].AssignmentId,
                    TimeWindowStart = result[i].TimeFrameStart,
                    TimeWindowEnd = result[i].TimeFrameEnd,
                    ArrivalTime = DateTime.MinValue
                };
                assignmentEntities.Add(temp);
            }
            return assignmentEntities;
        }

        public List<DistanceEntity> GetDistanceses(List<AssignmentEntity> assignmentsForPeriod)
        {
            var distancesQuery = _context.GetAll<DistanceDbModel>("DistanceCollection");

            List<DistanceEntity> result = distancesQuery.Select(distance => new DistanceEntity
            {
                AssignmentOne = distance.ResidenceOneId,
                AssignmentTwo = distance.ResidenceTwoId,
                DistanceBetween = distance.Distance
            }).ToList();

            return result;
        }

        public List<EmployeeEntity> GetAllEmployees()
        {
            var employeeQuery = _context.GetAll<EmployeeDbModel>("EmployeeCollection");

            List<EmployeeEntity> employees = employeeQuery.Select( employee => new EmployeeEntity
            {
                EmployeeId = employee.EmployeeId,
                Weeklyworkhours = employee.WeeklyWorkingHours,
                EmployeeType = employee.EmployeeTypeId,
            }).ToList();
         
            return employees;
        }

        public void AddEmployeesToRoute(List<EmployeeEntity> employees)
        {
            var employeesWithRoutes = employees.Where(e => e.Routes.Count > 0).ToList();
            foreach (var employee in employeesWithRoutes)
            {
                var employeeDbModel = _context.Get<EmployeeDbModel>(e => e.EmployeeId == employee.EmployeeId, "EmployeeCollection").FirstOrDefault();
                var employeeRoute = new EmployeeRouteDbModel(employeeDbModel);
                foreach (var route in employee.Routes)
                {
                    foreach (var assignment in route.Assignments)
                    {
                        var currentAssignment = _context.Get<AssignmentDbModel>((a => a.AssignmentId == assignment.Id), "AssignmentCollection").FirstOrDefault();
                        var currentCitizen = _context.Get<CitizenDbModel>(c => c.ResidenceId == currentAssignment.ResidenceId, "CitizenCollection").FirstOrDefault();
                        var employeeAssignment = new EmployeeAssignmentDbModel(assignment, currentCitizen, currentAssignment);
                        employeeRoute.Assignments.Add(employeeAssignment);
                    }
                }
                _context.Insert<EmployeeRouteDbModel>(employeeRoute, "RouteCollection");
            }
        }
        public bool ValidateUserExist(string username, string password)
        {
            var employeeMatchingUserInput = _context.Get<EmployeeDbModel>(x => x.Initials == username && x.EmployeePassword == password, "EmployeeCollection");

            if (employeeMatchingUserInput.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<EmployeeRouteDbModel> GetUsersWorkSchedule(string username, string password)
        {
            return _context.Get<EmployeeRouteDbModel>(r => r.Initials == username, "RouteCollection");
        }
        #endregion
    }
}
