//using Bogus;
//using ComfortCare.Api.Models;
//using ComfortCare.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ComfortCare.Api.Controllers
//{
//    //TODO: To be removed befor deployment to production
//    /// <summary>
//    /// This controller is used for testing purposes, only in the development phase, to make it 
//    /// possible to used the back and frontend together while the login controller is being created
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestController : ControllerBase
//    {
//        #region fields
//        private readonly ComfortCareDbContext _dbContext;
//        #endregion

//        #region Constructor
//        public TestController(ComfortCareDbContext dbContext)
//        {
//            this._dbContext = dbContext;
//        }
//        #endregion

//        #region Controller Methods

//        /// <summary>
//        /// This controller method is used to sent static test data to our 
//        /// frontend flutter application
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("LoginTestEmployee")]
//        public IActionResult LoginTestEmployee(LoginDto loginDto)
//        {
//            return Ok(GenerateDummyData());
//        }



//        [HttpPost("GetEmployeeRoutes")]
//        public IActionResult GetEmployeeRoutes([FromBody] GetRouteDto employeeId)
//        {
//            try
//            {
//                var response = GetRoutesForEmployee(employeeId.EmployeeId);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }


//        /// <summary>
//        /// This controller method is used to wipe the EmployeeRoute and RouteAssignment tables
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("WipeEmployeeRouteAndRouteAssignment")]
//        public IActionResult WipeEmployeeRouteAndRouteAssignment()
//        {
//            try
//            {
//                WipeRoutesAndAssignments();
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// This method is use to create dummy data in the development phase for the frontend,
//        /// it do not get any information from Db, it uses random add static data to simulate the
//        /// data need in the view for the users
//        /// </summary>
//        /// <returns>a data transfer object with dummy content in the development phase</returns>
//        private EmployeeScheduleDto GenerateDummyData()
//        {
//            var result = new EmployeeScheduleDto
//            {
//                Name = "Fiktiv bruger",
//                Assignments = new List<AssignmentDto>()
//            };

//            var citizenNames = new[] {
//                "Inger Hansen", "Erik Nielsen", "Lise Jensen", "Hans Pedersen", "Karen Møller",
//                "Anna Christensen", "Ole Larsen", "Mette Andersen", "Jens Pedersen", "Marie Nielsen",
//                "Anders Jensen", "Camilla Olsen", "Thomas Petersen", "Sofie Poulsen", "Michael Hansen"
//            };
//            var addresses = new[] {
//                "Ahornvej 5, Kværkeby ,4100 Ringsted", "Bøgevej 12,, 4100 Ringsted", "Egevej 8, Kværkeby, 4100 Ringsted",
//                "Fyrvej 15,,4100 Ringsted", "Granvej 3, Kværkeby, 4100 Ringsted", "Solsikkevej 7,, 4100 Ringsted",
//                "Kastanjevej 14, Kværkeby, 4100 Ringsted", "Engvej 22,, 4100 Ringsted", "Skovgade 9, Kværkeby, 4100 Ringsted",
//                "Rosenvej 2,, 4100 Ringsted", "Nygade 11, Kværkeby, 4100 Ringsted", "Møllegade 6, Kværkeby, 4100 Ringsted",
//                "Birkevej 18, Kværkeby, 4100 Ringsted", "Bakkevej 31,, 4100 Ringsted", "Søndre Ringgade 55,, 4100 Ringsted"
//            };
//            var assignmentTitles = new[] {
//                "Personlig pleje til ældre borger",
//                "Mobilisering og træning",
//                "Daglig medicinadministration",
//                "Ernæringsvejledning",
//                "Sårpleje og bandagering",
//                "Assistance ved måltider",
//                "Hjemmebesøg og socialt samvær",
//                "Indkøb og rengøring",
//                "Tilberedning af måltider"
//            };

//            var lorem = new Bogus.DataSets.Lorem();
//            var startDate = DateTime.Now.Date;
//            var random = new Randomizer();

//            for (int days = 0; days < 7; days++)
//            {
//                for (int assignmentsPrDay = 0; assignmentsPrDay < 5; assignmentsPrDay++)
//                {
//                    var assignment = new AssignmentDto
//                    {
//                        Titel = assignmentTitles[random.Number(0, assignmentTitles.Length - 1)],
//                        Description = lorem.Paragraphs(2),
//                        StartDate = startDate.AddHours(random.Number(0, 23)).AddMinutes(random.Number(0, 59)),
//                        CitizenName = citizenNames[random.Number(0, citizenNames.Length - 1)],
//                        Address = addresses[random.Number(0, addresses.Length - 1)],
//                    };

//                    assignment.EndDate = assignment.StartDate.AddSeconds(1800);

//                    result.Assignments.Add(assignment);
//                }

//                startDate = startDate.AddDays(1);
//            }

//            return result;
//        }

//        /// <summary>
//        /// This methos is wiping and resetting the EmployeeRoute and RouteAssignment tables
//        /// </summary>
//        private void WipeRoutesAndAssignments()
//        {
//            using (var transaction = _dbContext.Database.BeginTransaction())
//            {
//                try
//                {
//                    // Delete all records from the RouteAssignment table that reference EmployeeRoute records
//                    var routeAssignmentsToDelete = _dbContext.RouteAssignment
//                        .Where(ra => _dbContext.EmployeeRoute.Any(er => er.Id == ra.EmployeeRouteId));
//                    _dbContext.RouteAssignment.RemoveRange(routeAssignmentsToDelete);
//                    _dbContext.SaveChanges();

//                    // Delete all records from the EmployeeRoute table
//                    _dbContext.EmployeeRoute.RemoveRange(_dbContext.EmployeeRoute);
//                    _dbContext.SaveChanges();

//                    // Reset the ID for the EmployeeRoute table (specific to your database)
//                    _dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('EmployeeRoute', RESEED, 0);");

//                    // Reset the ID for the RouteAssignment table (specific to your database)
//                    _dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('RouteAssignment', RESEED, 0);");

//                    transaction.Commit();
//                }
//                catch
//                {
//                    transaction.Rollback();
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// Thismethod is use to get the the routes, assignments, how many each day and the total
//        /// </summary>
//        /// <param name="employeeId"></param>
//        /// <returns></returns>
//        /// <exception cref="Exception"></exception>
//        private EmployeeRouteDetailsDto GetRoutesForEmployee(int employeeId)
//        {
//            // Validate if the employee exists
//            var employeeExists = _dbContext.Employee.Any(e => e.Id == employeeId);
//            if (!employeeExists)
//            {
//                throw new Exception($"Employee with ID {employeeId} not found.");
//            }

//            // Fetch the routes for the employee
//            var routes = (from er in _dbContext.EmployeeRoute
//                          join ra in _dbContext.RouteAssignment on er.Id equals ra.EmployeeRouteId
//                          join a in _dbContext.Assignment on ra.AssignmentId equals a.Id
//                          join at in _dbContext.AssignmentType on a.AssignmentTypeId equals at.Id
//                          where er.EmployeeId == employeeId
//                          select new RouteDetailsDto
//                          {
//                              EmployeeRouteID = er.Id,
//                              RouteAssignmentID = ra.Id,
//                              AssignmentID = ra.AssignmentId,
//                              ArrivalTime = ra.ArrivalTime,
//                              EndTime = ra.ArrivalTime.AddSeconds(at.DurationInSeconds) // Calculating EndTime
//                          }).ToList();

//            // Calculate the total routes count by counting unique EmployeeRouteID
//            int totalRoutes = routes.Select(r => r.EmployeeRouteID).Distinct().Count();

//            // Calculate the daily assignments and routes count
//            var dailyAssignments = routes.GroupBy(r => new { r.ArrivalTime.Date, r.EmployeeRouteID })
//                                        .Select(g => new
//                                        {
//                                            Date = g.Key.Date,
//                                            RouteId = g.Key.EmployeeRouteID,
//                                            AssignmentsCount = g.Count()
//                                        })
//                                        .GroupBy(r => r.Date)
//                                        .Select(g => new DailyAssignmentsDto
//                                        {
//                                            Date = g.Key,
//                                            AssignmentsCount = g.Sum(a => a.AssignmentsCount),
//                                            RoutesCount = g.Count()
//                                        }).ToList();

//            // Prepare the response DTO
//            var response = new EmployeeRouteDetailsDto
//            {
//                EmployeeId = employeeId,
//                TotalRoutes = totalRoutes,
//                DailyAssignments = dailyAssignments,
//                Routes = routes
//            };

//            return response;
//        }

//        #endregion
//    }

//    #region DtoClasses used only in dev/test

//    /// <summary>
//    /// This class is used to get the route details
//    /// </summary>
//    public class GetRouteDto
//    {
//        public int EmployeeId { get; set; }
//    }


//    /// <summary>
//    /// This class is used to get the employee route details
//    /// </summary>
//    public class EmployeeRouteDetailsDto
//    {
//        public int EmployeeId { get; set; }
//        public int TotalRoutes { get; set; } // Total routes count
//        public List<DailyAssignmentsDto> DailyAssignments { get; set; } // Daily assignments count
//        public List<RouteDetailsDto> Routes { get; set; }
//    }


//    /// <summary>
//    /// This class is used to get the daily assignments
//    /// </summary>
//    public class DailyAssignmentsDto
//    {
//        public DateTime Date { get; set; }
//        public int AssignmentsCount { get; set; }
//        public int RoutesCount { get; set; } // Tilføjet denne linje
//    }

//    /// <summary>
//    /// This class is used to get the route details
//    /// </summary>
//    public class RouteDetailsDto
//    {
//        public int EmployeeRouteID { get; set; }
//        public int RouteAssignmentID { get; set; }
//        public int AssignmentID { get; set; }
//        public DateTime ArrivalTime { get; set; }
//        public DateTime EndTime { get; set; }
//    }
//    #endregion
//}