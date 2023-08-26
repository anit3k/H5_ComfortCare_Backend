using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic
{

    /// <summary>
    /// This class is used to populate routes with employees, it should use the rules for working hours
    /// and look at the individual employee to calculate their specific working time etc...
    /// </summary>
    public class SchemaGenerator
    {
        #region Fields
        private readonly IEmployeesRepo _employeesRepo;

        private HashSet<(int EmployeeId, DateTime RouteDay)> _assignedEmployees = new HashSet<(int, DateTime)>();
        #endregion

        #region Constructor 
        public SchemaGenerator(IEmployeesRepo employeesRepo)
        {
            _employeesRepo = employeesRepo;
        }
        #endregion


        #region Methods




        public void GenerateSchema(List<RouteEntity> routes)
        {
            //groups the list of routes by week, where the week starts on Monday.
            var groups = routes.GroupBy(r => r.RouteDate.Date.AddDays(-(int)r.RouteDate.DayOfWeek + (int)DayOfWeek.Monday)).Select(group => group.ToList()).ToList();


            foreach (var group in groups)
            {
                // Clear the assigned employees for the new week
                _assignedEmployees.Clear();
                //Splitting the routes into long and short routes
                var splitRoutes = SplitRoutesByTime(group);

                var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
                var employeesPartTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours < 40).ToList();
                var employeesSubstitutes = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours > 40).ToList();

                employeesFullTime.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesPartTime.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesSubstitutes.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);

                UpdateFourWeekWorkHours(employeesFullTime);
                UpdateFourWeekWorkHours(employeesPartTime);
                UpdateFourWeekWorkHours(employeesSubstitutes);

                var result = AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime, employeesSubstitutes);

                _employeesRepo.AddEmployeesToRoute(result);
            }
        }


        private List<List<RouteEntity>> SplitRoutesByTime(List<RouteEntity> routes)
        {
            var longRoutes = new List<RouteEntity>();
            var shortRoutes = new List<RouteEntity>();

            foreach (RouteEntity routeEntity in routes)
            {
                var totalTime = routeEntity.Assignments.Last().ArrivalTime - routeEntity.Assignments.First().ArrivalTime;
                if (totalTime > TimeSpan.FromHours(5))
                {
                    longRoutes.Add(routeEntity);
                }
                else
                {
                    shortRoutes.Add(routeEntity);
                }
            }
            return new List<List<RouteEntity>> { longRoutes, shortRoutes };
        }


        private void UpdateFourWeekWorkHours(List<EmployeeEntity> employees)
        {
            foreach (var employee in employees)
            {
                if (employee.PastFourWeeksWorkHoursInSeconds.Count >= 4)
                {
                    employee.PastFourWeeksWorkHoursInSeconds.Dequeue();
                }
                employee.PastFourWeeksWorkHoursInSeconds.Enqueue(employee.WorkhoursWithincurentWeekInSeconds);
            }
        }



        // This method assigns routes to employees based on their type and availability
        private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime, List<EmployeeEntity> employeesSubstitutes)
        {
            var employeesNeededForTheRoutes = new List<EmployeeEntity>();

            // Assign routes to full-time, part-time, and substitute employees
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime);
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesSubstitutes);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesSubstitutes);

            // Combine all employees who have been assigned routes
            employeesNeededForTheRoutes.AddRange(employeesFullTime);
            employeesNeededForTheRoutes.AddRange(employeesPartTime);
            employeesNeededForTheRoutes.AddRange(employeesSubstitutes);

            return employeesNeededForTheRoutes;
        }


        private void AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees)
        {
            // Dictionary to keep track of which days each employee has been assigned a route
            Dictionary<int, List<DateTime>> employeeRouteDays = new Dictionary<int, List<DateTime>>();

            while (routes.Any())
            {
                bool routeAssigned = false;

                foreach (var employee in employees)
                {
                    if (routes.Any())
                    {
                        var route = routes.First();
                        var routeDuration = CalculateRouteDuration(route);
                        var routeDay = route.RouteDate.Date;

                        // Initialize the List for this employee if it doesn't exist
                        if (!employeeRouteDays.ContainsKey(employee.EmployeeId))
                        {
                            employeeRouteDays[employee.EmployeeId] = new List<DateTime>();
                        }

                        if (!employee.WorkHoursPerDayInSeconds.ContainsKey(routeDay))
                        {
                            employee.WorkHoursPerDayInSeconds[routeDay] = 0;
                        }

                        double fourWeekAverage = (employee.PastFourWeeksWorkHoursInSeconds.Sum() + routeDuration) / (employee.PastFourWeeksWorkHoursInSeconds.Count + 1);

                        if (IsEmployeeAvailableForRoute(employee, routeDuration, fourWeekAverage, routeDay, employeeRouteDays[employee.EmployeeId]))
                        {
                            AssignRouteToEmployee(employee, route, routeDuration, routeDay);

                            // Add this employee and day to the list of assigned routes
                            employeeRouteDays[employee.EmployeeId].Add(routeDay);

                            routes.RemoveAt(0);
                            routeAssigned = true;
                        }
                    }
                }

                if (!routeAssigned)
                {
                    break;
                }
            }
        }




        // Check if the employee is available to take the route
        private bool IsEmployeeAvailableForRoute(EmployeeEntity employee, double routeDuration, double fourWeekAverage, DateTime routeDay, List<DateTime> assignedDays)
        {
            // Sort the assigned days to check for consecutive days off
            assignedDays.Sort();

            // Check if there are at least two consecutive days off
            bool hasTwoConsecutiveDaysOff = false;
            for (int i = 0; i <= 5; i++)
            {
                DateTime day = routeDay.Date.AddDays(-i);
                if (!assignedDays.Contains(day) && !assignedDays.Contains(day.AddDays(-1)))
                {
                    hasTwoConsecutiveDaysOff = true;
                    break;
                }
            }

            return hasTwoConsecutiveDaysOff &&
                   employee.WorkhoursWithincurentWeekInSeconds + routeDuration <= employee.Weeklyworkhours * 60 * 60 &&
                   fourWeekAverage <= employee.Weeklyworkhours * 60 * 60 &&
                   employee.WorkHoursPerDayInSeconds[routeDay] + routeDuration <= 12 * 60 * 60;
        }



        // Assign the route to the employee and update the relevant fields
        private void AssignRouteToEmployee(EmployeeEntity employee, RouteEntity route, double routeDuration, DateTime routeDay)
        {
            employee.Routes.Add(route);
            employee.WorkhoursWithincurentWeekInSeconds += routeDuration;
            employee.WorkHoursPerDayInSeconds[routeDay] += routeDuration;

            DateTime routeStartTime = route.Assignments.First().ArrivalTime;
            DateTime routeEndTime = route.Assignments.Last().ArrivalTime;

            if (!employee.WorkBlocksPerDay.ContainsKey(routeDay))
            {
                employee.WorkBlocksPerDay[routeDay] = new List<(DateTime Start, DateTime End)>();
            }
            employee.WorkBlocksPerDay[routeDay].Add((routeStartTime, routeEndTime));

        }


        // Calculate the total duration of a route
        private double CalculateRouteDuration(RouteEntity route)
        {
            // Calculate the total duration of the route based on the assignments
            return route.Assignments.Sum(a => a.Duration);
        }
        #endregion
    }
}


