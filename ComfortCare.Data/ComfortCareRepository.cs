using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;
using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Data
{
    /// <summary>
    /// This repository is used by the planmanager to read information from database and save routes calculated for a 
    /// statement period
    /// </summary>
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
        /// <summary>
        /// The method gets the number of assignments to be route calculated
        /// </summary>
        /// <param name="assignments">number of assignments from db</param>
        /// <returns>List of assignmentEntities</returns>
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

        /// <summary>
        /// Returns a list of distances between citizens that is connected to each other on the assignmentList.
        /// </summary>
        /// <param name="assignmentsForPeriod">A list of assignments within the valid period.</param>
        /// <returns>Returns a list of distances to calculate the shortest routes.</returns>
        public List<DistanceEntity> GetDistanceses(List<AssignmentEntity> assignmentsForPeriod)
        {
            var assignmentIds = assignmentsForPeriod.Select(a => a.Id).Distinct().ToList();
            var assignmentQuery = _context.Get<AssignmentDbModel>(a => assignmentIds.Contains(a.AssignmentId), "AssignmentCollection");
            var residenceIds = assignmentQuery.Select(a => a.ResidenceId).Distinct().ToList();
            var distancesQuery = _context.Get<DistanceDbModel>(d => residenceIds.Contains(d.ResidenceOneId) || residenceIds.Contains(d.ResidenceTwoId), "DistanceCollection");

            List<DistanceEntity> result = distancesQuery.Select(distance => new DistanceEntity
            {
                AssignmentOne = distance.ResidenceOneId,
                AssignmentTwo = distance.ResidenceTwoId,
                DistanceBetween = distance.Distance
            }).ToList();

            return result;
        }


        /// <summary>
        /// This method is getting a complete list of all the employeeRoute from the db
        /// and maps it to the domain entity of an employeeRoute, this list is used in the employeeRoute
        /// algorithm to calculate which employeeRoute can have witch routes.
        /// </summary>
        /// <returns>A List of all the employeeRoute entities</returns>
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

        /// <summary>
        /// This method maps and saves routes to employees in the database
        /// </summary>
        /// <param name="employees"></param>
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

        /// <summary>
        /// this method validate wheter a user exist in the db
        /// </summary>
        /// <param name="username">the users Initials</param>
        /// <param name="password">the users password</param>
        /// <returns>Returns a boolean, if user exist true, and if not false</returns>
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

        /// <summary>
        /// This method is used to get all the information needed for the current
        /// employeeRoute to show in the UI
        /// </summary>
        /// <param name="username">the users Initials</param>
        /// <param name="password">the users password</param>
        /// <returns>An employeeRoute db model object, that contains all the information needed for the route and assignments</returns>
        public List<EmployeeRouteDbModel> GetUsersWorkSchedule(string username, string password)
        {
            return _context.Get<EmployeeRouteDbModel>(r => r.Initials == username, "RouteCollection");
        }
        #endregion
    }
}
