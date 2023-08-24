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
    public class ComfortCareRepository : IRouteRepo, IEmployeesRepo
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

        // TODO: refactor these method to fit what ever the route calculator algorithm needs.

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

        public void AddEmployeesToRoute(List<EmployeeEntity> employees)
        {
            throw new NotImplementedException();
        }


        public void InsertRoutes(List<EmployeeEntity> employees)
        {
            // Loop through all employees adding their routes to the database
            foreach (var employee in employees)
            {
                var route = employee.Route;
                if (route != null && route.Assignments != null)
                {
                    foreach (var assignment in route.Assignments)
                    {
                        // Creates and inserts EmployeeRoute
                        var employeeRoute = new EmployeeRoute
                        {
                            AssignmentId = assignment.Id,
                            ArrivalTime = assignment.ArrivalTime
                        };
                        _context.EmployeeRoute.Add(employeeRoute);
                        _context.SaveChanges(); // Save changes to generate the Id for employeeRoute

                        // Creates and inserts EmployeeEmployeeRoute
                        var employeeEmployeeRoute = new EmployeeEmployeeRoute
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeeRouteId = employeeRoute.Id // Now this Id is valid
                        };
                        _context.EmployeeEmployeeRoute.Add(employeeEmployeeRoute);
                    }
                }
            }
            // Saving changes to Database
            _context.SaveChanges();
        } 

        #endregion
    }
}
