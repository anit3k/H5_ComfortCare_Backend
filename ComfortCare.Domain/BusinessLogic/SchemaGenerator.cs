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

                var splitRoutes = SplitRoutesByTime(group);

                var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
                var employeesPartTime30Hours = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 30).ToList();
                var employeesPartTime25Hours = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 25).ToList();
                var employeesSubstitutes = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours > 40).ToList();

                employeesFullTime.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesPartTime30Hours.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesPartTime25Hours.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesSubstitutes.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);

                UpdateFourWeekWorkHours(employeesFullTime);
                UpdateFourWeekWorkHours(employeesPartTime30Hours);
                UpdateFourWeekWorkHours(employeesPartTime25Hours);
                UpdateFourWeekWorkHours(employeesSubstitutes);

                var result = AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime30Hours, employeesPartTime25Hours, employeesSubstitutes);

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
            // Loop through each employee in the list
            foreach (var employee in employees)
            {
                // Check if the employee's work hours for the past four weeks are already stored
                if (employee.PastFourWeeksWorkHoursInSeconds.Count >= 4)
                {
                    // Remove the oldest work hours data to make room for the new week
                    employee.PastFourWeeksWorkHoursInSeconds.Dequeue();
                }

                // Add the current week's work hours to the queue
                employee.PastFourWeeksWorkHoursInSeconds.Enqueue(employee.WorkhoursWithincurentWeekInSeconds);
            }
        }



        // This method assigns routes to employees based on their type and availability
        private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime30Hours, List<EmployeeEntity> employeesPartTime25Hours, List<EmployeeEntity> employeesSubstitutes)
        {
            var employeesNeededForTheRoutes = new List<EmployeeEntity>();

            // Assign routes to full-time and part-time 30 hours, part-time 25 hours, and substitute employees
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime);
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesPartTime30Hours);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime25Hours);
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesSubstitutes);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesSubstitutes);

            // Combine all employees who have been assigned routes
            employeesNeededForTheRoutes.AddRange(employeesFullTime);
            employeesNeededForTheRoutes.AddRange(employeesPartTime30Hours);
            employeesNeededForTheRoutes.AddRange(employeesPartTime25Hours);
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

                // Sort employees by their already assigned work hours to prioritize employees already applied workinghours
                employees = employees.OrderBy(e => e.WorkhoursWithincurentWeekInSeconds).ToList();

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


        // Calculate the total duration of a route
        private double CalculateRouteDuration(RouteEntity route)
        {
            // Calculate the total duration of the route based on the assignments
            return route.Assignments.Sum(a => a.Duration);
        }


        private bool IsEmployeeAvailableForRoute(EmployeeEntity employee, double routeDuration, double fourWeekAverage, DateTime routeDay, List<DateTime> assignedDays)
        {
            // Constants
            const double RequiredHoursOff = 55 * 60 * 60;  // 55 hours converted to seconds

            // Check if the employee's work hours for the current week and route duration do not exceed the weekly limit
            bool withinWeeklyLimit = employee.WorkhoursWithincurentWeekInSeconds + routeDuration <= employee.Weeklyworkhours * 60 * 60;

            // Check if the four-week average work hours do not exceed the weekly limit
            bool withinFourWeekLimit = fourWeekAverage <= employee.Weeklyworkhours * 60 * 60;

            // Check if the employee's work hours for the given day and route duration do not exceed the daily limit of 12 hours
            bool withinDailyLimit = employee.WorkHoursPerDayInSeconds[routeDay] + routeDuration <= 12 * 60 * 60;

            // Initialize variable to keep track of consecutive free time in seconds
            double consecutiveFreeTimeInSeconds = 0;

            // Initialize variable to keep track of whether the employee has the required free time
            bool hasRequiredFreeTime = true;

            // Sort WorkingDaysList by RouteStart
            employee.WorkingDaysList.Sort((a, b) => a.RouteStart.CompareTo(b.RouteStart));

            // Ensure the list only contains the last 7 days
            while (employee.WorkingDaysList.Count > 7)
            {
                employee.WorkingDaysList.RemoveAt(0);
            }

            // Initialize variable to keep track of the last work day's end time
            DateTime lastWorkDayEndTime = DateTime.MinValue;

            // Check for 55 consecutive free hours within the last 7 days
            foreach (var workDay in employee.WorkingDaysList)
            {
                if (lastWorkDayEndTime != DateTime.MinValue)
                {
                    var timeDifferenceInSeconds = (workDay.RouteStart - lastWorkDayEndTime).TotalSeconds;
                    if (timeDifferenceInSeconds > RequiredHoursOff)
                    {
                        consecutiveFreeTimeInSeconds = timeDifferenceInSeconds;
                        break;
                    }
                }

                lastWorkDayEndTime = workDay.RouteEnd;
            }
            if (employee.WorkingDaysList.Count > 5)
            {
                // Check if the employee had 55 consecutive free hours
                hasRequiredFreeTime = consecutiveFreeTimeInSeconds >= RequiredHoursOff;

            }

            // Return true only if all conditions are met
            return withinWeeklyLimit && withinFourWeekLimit && withinDailyLimit && hasRequiredFreeTime;
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
            //Adding ruteInformation to the WorkingdayList
            employee.WorkingDaysList.Add(new TimeSpanEntity { RouteStart = routeStartTime, RouteEnd = routeEndTime });

        }



        #endregion
    }
}


