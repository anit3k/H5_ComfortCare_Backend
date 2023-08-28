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




        /// <summary>
        /// This method Generates a schema for assigning routes to employees based on their availability and type. This is the main method to call the rest.
        /// </summary>
        /// <param name="routes">List of routes to be assigned.</param>
        public void GenerateSchema(List<RouteEntity> routes)
        {
            // Group the list of routes by week, where the week starts on Monday
            var groups = routes.GroupBy(r => r.RouteDate.Date.AddDays(-(int)r.RouteDate.DayOfWeek + (int)DayOfWeek.Monday)).Select(group => group.ToList()).ToList();

            // Loop through each group of routes (each week)
            foreach (var group in groups)
            {
                // Split the routes into long and short routes
                var splitRoutes = SplitRoutesByTime(group);

                // Fetch all employees from the database and categorize them based on their weekly work hours
                var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
                var employeesPartTime30Hours = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 30).ToList();
                var employeesPartTime25Hours = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 25).ToList();
                var employeesSubstitutes = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours > 40).ToList();

                // Reset the current week's work hours for all employees
                employeesFullTime.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesPartTime30Hours.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesPartTime25Hours.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
                employeesSubstitutes.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);

                // Update the work hours for the past four weeks for each category of employees
                UpdateFourWeekWorkHours(employeesFullTime);
                UpdateFourWeekWorkHours(employeesPartTime30Hours);
                UpdateFourWeekWorkHours(employeesPartTime25Hours);
                UpdateFourWeekWorkHours(employeesSubstitutes);

                // Assign routes to employees and get the list of employees who have been assigned routes
                var result = AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime30Hours, employeesPartTime25Hours, employeesSubstitutes);

                // Add the employees to the routes in the database
                _employeesRepo.AddEmployeesToRoute(result);
            }
        }



        /// <summary>
        /// This method Splits a list of routes into two categories: long routes and short routes.
        /// </summary>
        /// <param name="routes">List of routes to be split.</param>
        /// <returns>A list containing two lists: one for long routes and one for short routes.</returns>
        private List<List<RouteEntity>> SplitRoutesByTime(List<RouteEntity> routes)
        {
            // Initialize lists to hold long and short routes
            var longRoutes = new List<RouteEntity>();
            var shortRoutes = new List<RouteEntity>();

            // Loop through each route in the list
            foreach (RouteEntity routeEntity in routes)
            {
                // Calculate the total time for the route
                var totalTime = routeEntity.Assignments.Last().ArrivalTime - routeEntity.Assignments.First().ArrivalTime;

                // Check if the route is longer than 5 hours
                if (totalTime > TimeSpan.FromHours(5))
                {
                    longRoutes.Add(routeEntity);
                }
                else
                {
                    shortRoutes.Add(routeEntity);
                }
            }

            // Return the lists of long and short routes
            return new List<List<RouteEntity>> { longRoutes, shortRoutes };
        }




        /// <summary>
        /// This method Updates the work hours for the past four weeks for each employee.
        /// </summary>
        /// <param name="employees">List of employees whose work hours need to be updated.</param>
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




        /// <summary>
        /// This method Assigns routes to employees based on their type (Full-Time, Part-Time 30 Hours, Part-Time 25 Hours, Substitute) and availability.
        /// </summary>
        /// <param name="splitRoutes">List of routes, split based on some criteria.</param>
        /// <param name="employeesFullTime">List of full-time employees.</param>
        /// <param name="employeesPartTime30Hours">List of part-time employees with 30 hours per week.</param>
        /// <param name="employeesPartTime25Hours">List of part-time employees with 25 hours per week.</param>
        /// <param name="employeesSubstitutes">List of substitute employees.</param>
        /// <returns>List of employees who have been assigned routes.</returns>
        private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime30Hours, List<EmployeeEntity> employeesPartTime25Hours, List<EmployeeEntity> employeesSubstitutes)
        {
            // Initialize a list to keep track of employees who are needed for the routes
            var employeesNeededForTheRoutes = new List<EmployeeEntity>();

            // Assign long routes to full-time employees
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime);

            // Assign long routes to part-time 30 hours employees
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesPartTime30Hours);

            // Assign short routes to part-time 25 hours employees
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime25Hours);

            // Assign both long and short routes to substitute employees if there is no more normal employees left
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesSubstitutes);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesSubstitutes);

            // Combine all employees who have been assigned routes into a single list
            employeesNeededForTheRoutes.AddRange(employeesFullTime);
            employeesNeededForTheRoutes.AddRange(employeesPartTime30Hours);
            employeesNeededForTheRoutes.AddRange(employeesPartTime25Hours);
            employeesNeededForTheRoutes.AddRange(employeesSubstitutes);

            // Return the list of employees who have been assigned routes
            return employeesNeededForTheRoutes;
        }


        /// <summary>
        /// This method Assigns routes to specific employees based on their availability and work hours.
        /// </summary>
        /// <param name="routes">List of routes to be assigned.</param>
        /// <param name="employees">List of employees to whom routes can be assigned to.</param>
        private void AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees)
        {
            // Flag to indicate whether a route has been successfully assigned in the current iteration
            bool routeAssigned = false;

            // Loop until all routes are assigned
            while (routes.Any())
            {
                // Sort employees by their already assigned work hours to prioritize those with fewer hours
                employees = employees.OrderBy(e => e.WorkhoursWithincurentWeekInSeconds).ToList();

                // Iterate through each employee to try and assign a route
                foreach (var employee in employees)
                {
                    // Check if there are still routes to assign
                    if (routes.Any())
                    {
                        // Get the first route and its details
                        var route = routes.First();
                        var routeDuration = CalculateRouteDuration(route);
                        var routeDay = route.RouteDate.Date;

                        // Initialize work hours for the route day if not already present
                        if (!employee.WorkHoursPerDayInSeconds.ContainsKey(routeDay))
                        {
                            employee.WorkHoursPerDayInSeconds[routeDay] = 0;
                        }

                        // Calculate the four-week average work hours for the employee
                        double fourWeekAverage = (employee.PastFourWeeksWorkHoursInSeconds.Sum() + routeDuration) / (employee.PastFourWeeksWorkHoursInSeconds.Count + 1);

                        // Check if the employee is available to take the route
                        if (IsEmployeeAvailableForRoute(employee, routeDuration, fourWeekAverage, routeDay))
                        {
                            // Assign the route to the employee
                            AssignRouteToEmployee(employee, route, routeDuration, routeDay);

                            // Remove the assigned route from the list
                            routes.RemoveAt(0);

                            // Set the flag to indicate a successful assignment
                            routeAssigned = true;
                        }
                    }
                }

                // Break the loop if no route could be assigned in the current iteration
                if (!routeAssigned)
                {
                    break;
                }
            }
        }



        /// <summary>
        /// This method is used to calculate the total duration of a route
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private double CalculateRouteDuration(RouteEntity route)
        {
            return route.Assignments.Sum(a => a.Duration);
        }


        /// <summary>
        /// This method is used to check if an employee is available for a specific route
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="routeDuration"></param>
        /// <param name="fourWeekAverage"></param>
        /// <param name="routeDay"></param>
        /// <returns></returns>
        private bool IsEmployeeAvailableForRoute(EmployeeEntity employee, double routeDuration, double fourWeekAverage, DateTime routeDay)
        {
            // Constants
            const double RequiredHoursOff = 55 * 60 * 60;  // 55 hours converted to seconds

            // Initialize variable to keep track of consecutive free time in seconds
            double consecutiveFreeTimeInSeconds = 0;

            // Initialize variable to keep track of whether the employee has the required free time
            bool hasRequiredFreeTime = true;


            // Check if the employee's work hours for the current week and route duration do not exceed the weekly limit
            bool withinWeeklyLimit = employee.WorkhoursWithincurentWeekInSeconds + routeDuration <= employee.Weeklyworkhours * 60 * 60;

            // Check if the four-week average work hours do not exceed the weekly limit
            bool withinFourWeekLimit = fourWeekAverage <= employee.Weeklyworkhours * 60 * 60;

            // Check if the employee's work hours for the given day and route duration do not exceed the daily limit of 12 hours
            bool withinDailyLimit = employee.WorkHoursPerDayInSeconds[routeDay] + routeDuration <= 12 * 60 * 60;

            //TODO: Remove in production
            if (employee.EmployeeId == 4)
            {
                Console.WriteLine("employeeID: " + employee.EmployeeId);
            }
            // Check if the employee has already a route assigned for the current day.
            bool isEmployeeFreeToBeAssignedToRoute = !employee.Routes.Any(r => r.RouteDate.Date == routeDay);

            // Sort WorkingDaysList in the EmployeeEntity by RouteStart
            employee.WorkingDaysList.Sort((a, b) => a.RouteStart.CompareTo(b.RouteStart));

            // Ensure the list only contains the last 7 days
            while (employee.WorkingDaysList.Count > 7)
            {
                employee.WorkingDaysList.RemoveAt(0);
            }

            // Initialize variable to keep track of the last work day's end time
            DateTime lastWorkDayEndTime = DateTime.MinValue;

            //TODO: Remove unused code before deployment
            //// Check for 55 consecutive free hours within the last 7 days (NOT WORKING)
            //foreach (var workDay in employee.WorkingDaysList)
            //{

            //    if (workDay.RouteEnd != DateTime.MinValue)
            //    {
            //        var timeDifferenceInSeconds = (workDay.RouteStart - lastWorkDayEndTime).TotalSeconds;
            //        if (timeDifferenceInSeconds > RequiredHoursOff)
            //        {
            //            consecutiveFreeTimeInSeconds = timeDifferenceInSeconds;
            //            break;
            //        }
            //        lastWorkDayEndTime = workDay.RouteEnd;
            //    }

            //    lastWorkDayEndTime = workDay.RouteEnd;
            //}
            //if (employee.WorkingDaysList.Count > 5)
            //{
            //    // Check if the employee had 55 consecutive free hours
            //    hasRequiredFreeTime = consecutiveFreeTimeInSeconds >= RequiredHoursOff;

            //}

            // Return true only if all conditions are met and the employee is not already assigned on the same day
            return withinWeeklyLimit && withinFourWeekLimit && withinDailyLimit && hasRequiredFreeTime && isEmployeeFreeToBeAssignedToRoute;
        }



        /// <summary>
        /// This method Assigns a route to an employee and updates relevant fields.
        /// </summary>
        /// <param name="employee">The employee to whom the route will be assigned.</param>
        /// <param name="route">The route to be assigned.</param>
        /// <param name="routeDuration">The duration of the route in seconds.</param>
        /// <param name="routeDay">The day on which the route is assigned.</param>
        private void AssignRouteToEmployee(EmployeeEntity employee, RouteEntity route, double routeDuration, DateTime routeDay)
        {
            // Add the route to the employee's list of routes
            employee.Routes.Add(route);

            // Update the total work hours for the current week by adding the route duration
            employee.WorkhoursWithincurentWeekInSeconds += routeDuration;

            // Update the work hours for the specific day by adding the route duration
            employee.WorkHoursPerDayInSeconds[routeDay] += routeDuration;

            // Get the start and end times of the route from the first and last assignments
            DateTime routeStartTime = route.Assignments.First().ArrivalTime;
            DateTime routeEndTime = route.Assignments.Last().ArrivalTime;

            // Initialize the work block list for the day if it doesn't exist
            if (!employee.WorkBlocksPerDay.ContainsKey(routeDay))
            {
                employee.WorkBlocksPerDay[routeDay] = new List<(DateTime Start, DateTime End)>();
            }

            // Add the route's start and end times to the work blocks for the day
            employee.WorkBlocksPerDay[routeDay].Add((routeStartTime, routeEndTime));

            // Add the route information to the WorkingDaysList
            employee.WorkingDaysList.Add(new TimeSpanEntity { RouteStart = routeStartTime, RouteEnd = routeEndTime });
        }




        #endregion
    }
}


