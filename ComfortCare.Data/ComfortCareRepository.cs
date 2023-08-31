using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;
using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComfortCare.Data
{
    /// <summary>
    /// This repository is used by the planmanager to read information from database and save routes calculated for a 
    /// statement period
    /// </summary>
    public class ComfortCareRepository : IRouteRepo, IEmployeesRepo, IUserRepo
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
        /// <summary>
        /// The method gets the number of assignments to be route calculated
        /// </summary>
        /// <param name="assignments">number of assignments from db</param>
        /// <returns>List of assignmentEntities</returns>
        public List<AssignmentEntity> GetNumberOfAssignments(int assignments)
        {
            var result = _context.Assignment.Include(a => a.AssignmentType).ThenInclude(at => at.TimeFrame).ToList();
            List<AssignmentEntity> assignmentEntities = new List<AssignmentEntity>();

            for (int i = 0; i < assignments; i++)
            {
                var temp = new AssignmentEntity()
                {
                    Duration = result[i].AssignmentType.DurationInSeconds,
                    Id = result[i].Id,
                    TimeWindowStart = result[i].AssignmentType.TimeFrame.TimeFrameStart,
                    TimeWindowEnd = result[i].AssignmentType.TimeFrame.TimeFrameEnd,
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
            List<int> assignmentIds = assignmentsForPeriod.Select(a => a.Id).ToList();
            var distancesQuery = _context.Distance.Where(d => assignmentIds.Contains(d.ResidenceOneId) && assignmentIds.Contains(d.ResidenceTwoId)).ToList();
            List<DistanceEntity> result = new List<DistanceEntity>();
            foreach (var distance in distancesQuery)
            {
                var temp = new DistanceEntity()
                {
                    AssignmentOne = distance.ResidenceOneId,
                    AssignmentTwo = distance.ResidenceTwoId,
                    DistanceBetween = distance.Duration
                };

                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// This method is getting a complete list of all the employee from the db
        /// and maps it to the domain entity of an employee, this list is used in the employee
        /// algorithm to calculate which employee can have witch routes.
        /// </summary>
        /// <returns>A List of all the employee entities</returns>
        public List<EmployeeEntity> GetAllEmployees()
        {
            var employeeQuery = _context.Employee
                .Include(ep => ep.EmployeePreference)
                    .ThenInclude(p => p.Preference)
                    .ThenInclude(wt => wt.WorkingTimespan)
                .Include(ep => ep.EmployeePreference)
                    .ThenInclude(p => p.Preference)
                    .ThenInclude(dt => dt.DayType)
                .Include(et => et.EmployeeType)
                .Include(es => es.EmployeeStatementPeriod)
                    .ThenInclude(sp => sp.StatementPeriod)
                .Include(es => es.EmployeeStatementPeriod)
                    .ThenInclude(tr => tr.TimeRegistration);


            List<EmployeeEntity> employees = new();
            foreach (var employee in employeeQuery)
            {
                var temp = new EmployeeEntity()
                {
                    EmployeeId = employee.Id,
                    Weeklyworkhours = employee.WeeklyWorkingHours,
                    EmployeeType = employee.EmployeeTypeId,
                };
                employees.Add(temp);
            }
            return employees;
        }
       
        /// <summary>
        /// This method maps and saves routes to employees in the database
        /// </summary>
        /// <param name="employees"></param>
        public void AddEmployeesToRoute(List<EmployeeEntity> employees)
        {
            foreach (var employee in employees)
            {
                var employeeQuery = _context.Employee.Where(e => e.Id == employee.EmployeeId).FirstOrDefault();

                foreach (var route in employee.Routes)
                {
                    var employeeRoute = new EmployeeRoute()
                    {
                        Employee = employeeQuery,
                    };

                    _context.EmployeeRoute.Add(employeeRoute);

                    foreach (var assignment in route.Assignments)
                    {
                        var employeeAssignment = new RouteAssignment()
                        {
                            EmployeeRoute = employeeRoute,
                            Assignment = _context.Assignment.Where(a => a.Id == assignment.Id).FirstOrDefault(),
                            ArrivalTime = assignment.ArrivalTime
                        };

                        employeeRoute.RouteAssignment.Add(employeeAssignment);
                    }
                }
            }
            _context.SaveChanges();
        }     

        /// <summary>
        /// this method validate wheter a user exist in the db
        /// </summary>
        /// <param name="username">the users initials</param>
        /// <param name="password">the users password</param>
        /// <returns>Returns a boolean, if user exist true, and if not false</returns>
        public bool ValidateUserExist(string username, string password)
        {
            var employeeMatchingUserInput = _context.Employee.Where(e => e.Initials == username && e.EmployeePassword == password).ToList();

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
        /// employee to show in the UI
        /// </summary>
        /// <param name="username">the users initials</param>
        /// <param name="password">the users password</param>
        /// <returns>An employee db model object, that contains all the information needed for the route and assignments</returns>
        public Employee GetUsersWorkSchedule(string username, string password)
        {
            var employee = _context.Employee
            .Include(e => e.EmployeeRoute)
                .ThenInclude(route => route.RouteAssignment)
                    .ThenInclude(routeAssignment => routeAssignment.Assignment)
                        .ThenInclude(assignment => assignment.AssignmentType)
            .Include(e => e.EmployeeRoute)
                .ThenInclude(route => route.RouteAssignment)
                    .ThenInclude(routeAssignment => routeAssignment.Assignment)
                        .ThenInclude(assignment => assignment.Citizen)
                            .ThenInclude(citizen => citizen.Residence)
            .FirstOrDefault(e => e.Initials == username && e.EmployeePassword == password);

            return employee;            
        }
        #endregion
    }
}
