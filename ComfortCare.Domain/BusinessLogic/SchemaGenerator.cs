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

        //TODO: Kent, add logic to ensure that it has been at least 11 hours ago since employ has been working
        //TODO: Kent - add logic to check for employees who have not worked within other timespan for 48 hours within this period
        #region Methods




        //public void GenerateSchema(List<RouteEntity> routes)
        //{
        //    // Group routes by week
        //    var groups = routes.GroupBy(r => r.RouteDate.Date.AddDays(-(int)r.RouteDate.DayOfWeek + (int)DayOfWeek.Monday))
        //                       .Select(group => group.ToList())
        //                       .ToList();

        //    foreach (var group in groups)
        //    {
        //        var splitRoutes = SplitRoutesByTime(group);

        //        // Fetch employees based on their work hours
        //        var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
        //        var employeesPartTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours < 40).ToList();
        //        var employeesSubstitutes = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours > 40).ToList();

        //        // Reset work hours for the current week
        //        employeesFullTime.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
        //        employeesPartTime.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);
        //        employeesSubstitutes.ForEach(e => e.WorkhoursWithincurentWeekInSeconds = 0);

        //        // Update four-week work hours history
        //        UpdateFourWeekWorkHours(employeesFullTime);
        //        UpdateFourWeekWorkHours(employeesPartTime);
        //        UpdateFourWeekWorkHours(employeesSubstitutes);

        //        // Assign routes to employees
        //        var result = AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime, employeesSubstitutes);

        //        _employeesRepo.AddEmployeesToRoute(result);
        //    }
        //}

        //private void UpdateFourWeekWorkHours(List<EmployeeEntity> employees)
        //{
        //    // Update the work hours history for the past four weeks
        //    foreach (var employee in employees)
        //    {
        //        if (employee.PastFourWeeksWorkHoursInSeconds.Count >= 4)
        //        {
        //            employee.PastFourWeeksWorkHoursInSeconds.Dequeue();
        //        }
        //        employee.PastFourWeeksWorkHoursInSeconds.Enqueue(employee.WorkhoursWithincurentWeekInSeconds);
        //    }
        //}

        //private List<List<RouteEntity>> SplitRoutesByTime(List<RouteEntity> routes)
        //{
        //    // Split routes into long and short based on their duration
        //    var longRoutes = new List<RouteEntity>();
        //    var shortRoutes = new List<RouteEntity>();

        //    foreach (RouteEntity routeEntity in routes)
        //    {
        //        var totalTime = routeEntity.Assignments.Last().ArrivalTime - routeEntity.Assignments.First().ArrivalTime;
        //        if (totalTime > TimeSpan.FromHours(5))
        //        {
        //            longRoutes.Add(routeEntity);
        //        }
        //        else
        //        {
        //            shortRoutes.Add(routeEntity);
        //        }
        //    }

        //    return new List<List<RouteEntity>> { longRoutes, shortRoutes };
        //}

        //private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime, List<EmployeeEntity> employeesSubstitutes)
        //{
        //    // Assign routes to employees based on their availability and type
        //    var employeesNeededForTheRoutes = new List<EmployeeEntity>();

        //    AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime, 55);
        //    AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime, 55);
        //    AssignRoutesToSpecificEmployees(splitRoutes[0], employeesSubstitutes, 0);
        //    AssignRoutesToSpecificEmployees(splitRoutes[1], employeesSubstitutes, 0);

        //    employeesNeededForTheRoutes.AddRange(employeesFullTime);
        //    employeesNeededForTheRoutes.AddRange(employeesPartTime);
        //    employeesNeededForTheRoutes.AddRange(employeesSubstitutes);

        //    return employeesNeededForTheRoutes;
        //}

        //private void AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees, int minFreeHours)
        //{
        //    while (routes.Count > 0)
        //    {
        //        bool routeAssigned = false;

        //        foreach (var employee in employees)
        //        {
        //            if (routes.Count > 0)
        //            {
        //                var route = routes[0];
        //                var routeDuration = CalculateRouteDuration(route);
        //                var routeDay = route.RouteDate.Date;
        //                var routeStartTime = route.Assignments.First().ArrivalTime;
        //                var routeEndTime = route.Assignments.Last().ArrivalTime.AddSeconds(routeDuration);

        //                if (!employee.WorkHoursPerDayInSeconds.ContainsKey(routeDay))
        //                {
        //                    employee.WorkHoursPerDayInSeconds[routeDay] = 0;
        //                    employee.WorkBlocksPerDay[routeDay] = new List<(TimeSpan, TimeSpan)>();
        //                }

        //                double fourWeekAverage = (employee.PastFourWeeksWorkHoursInSeconds.Sum() + routeDuration) / (employee.PastFourWeeksWorkHoursInSeconds.Count + 1);

        //                int maxWorkHoursPerWeekInSeconds = employee.Weeklyworkhours * 60 * 60;

        //                // Check for 55-hour continuous free time
        //                bool hasEnoughFreeTime = CheckForContinuousFreeTime(employee.WorkBlocksPerDay, minFreeHours);

        //                if (employee.WorkhoursWithincurentWeekInSeconds + routeDuration <= maxWorkHoursPerWeekInSeconds &&
        //                    fourWeekAverage <= maxWorkHoursPerWeekInSeconds &&
        //                    employee.WorkHoursPerDayInSeconds[routeDay] + routeDuration <= 12 * 60 * 60 &&
        //                    hasEnoughFreeTime)
        //                {
        //                    employee.Routes.Add(route);
        //                    employee.WorkhoursWithincurentWeekInSeconds += routeDuration;
        //                    employee.WorkHoursPerDayInSeconds[routeDay] += routeDuration;
        //                    employee.WorkBlocksPerDay[routeDay].Add((routeStartTime.TimeOfDay, routeEndTime.TimeOfDay));

        //                    routes.RemoveAt(0);
        //                    routeAssigned = true;
        //                }
        //            }
        //        }

        //        if (!routeAssigned)
        //        {
        //            break;
        //        }
        //    }
        //}


        //private bool CheckForContinuousFreeTime(Dictionary<DateTime, List<(TimeSpan Start, TimeSpan End)>> workBlocksPerDay, int minFreeHours)
        //{
        //    // Sort work blocks and check for continuous free time
        //    foreach (var day in workBlocksPerDay.Keys)
        //    {
        //        var workBlocks = workBlocksPerDay[day];
        //        workBlocks.Sort((a, b) => a.Start.CompareTo(b.Start));

        //        TimeSpan lastEndTime = TimeSpan.Zero;

        //        foreach (var block in workBlocks)
        //        {
        //            var freeTime = block.Start - lastEndTime;
        //            if (freeTime.TotalHours >= minFreeHours)
        //            {
        //                return true;
        //            }
        //            lastEndTime = block.End;
        //        }

        //        // Check free time after the last work block until midnight
        //        var remainingTime = TimeSpan.FromHours(24) - lastEndTime;
        //        if (remainingTime.TotalHours >= minFreeHours)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}


        //private double CalculateRouteDuration(RouteEntity route)
        //{
        //    // Calculate the total duration of the route based on the assignments
        //    return route.Assignments.Sum(a => a.Duration);
        //}





        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------







        public void GenerateSchema(List<RouteEntity> routes)
        {
            var groups = routes.GroupBy(r => r.RouteDate.Date.AddDays(-(int)r.RouteDate.DayOfWeek + (int)DayOfWeek.Monday)).Select(group => group.ToList()).ToList();

            foreach (var group in groups)
            {
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

        // This method assigns routes to employees based on their type and availability
        private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime, List<EmployeeEntity> employeesSubstitutes)
        {
            var employeesNeededForTheRoutes = new List<EmployeeEntity>();

            // Assign routes to full-time, part-time, and substitute employees
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime);
            //AssignRoutesToSpecificEmployees(splitRoutes[1], employeesFullTime);
            //AssignRoutesToSpecificEmployees(splitRoutes[0], employeesPartTime);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime);
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesSubstitutes);
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesSubstitutes);

            // Combine all employees who have been assigned routes
            employeesNeededForTheRoutes.AddRange(employeesFullTime);
            employeesNeededForTheRoutes.AddRange(employeesPartTime);
            employeesNeededForTheRoutes.AddRange(employeesSubstitutes);

            return employeesNeededForTheRoutes;
        }

        // Modified AssignRoutesToSpecificEmployees method
        private void AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees)
        {
            while (routes.Count > 0)
            {
                bool routeAssigned = false;

                foreach (var employee in employees)
                {
                    if (routes.Count > 0)
                    {
                        var route = routes[0];
                        var routeDuration = CalculateRouteDuration(route);
                        var routeDay = route.RouteDate.Date;

                        if (!employee.WorkHoursPerDayInSeconds.ContainsKey(routeDay))//Ensuring that only one run is added per day
                        {
                            employee.WorkHoursPerDayInSeconds[routeDay] = 0;
                        }
                        {
                            employee.WorkHoursPerDayInSeconds[routeDay] = 0;
                        }
                        //Calculating the average work hours per week over the last 4 weeks
                        double fourWeekAverage = (employee.PastFourWeeksWorkHoursInSeconds.Sum() + routeDuration) / (employee.PastFourWeeksWorkHoursInSeconds.Count + 1);

                        if (employee.WorkhoursWithincurentWeekInSeconds + routeDuration <= employee.Weeklyworkhours * 60 * 60 &&//Ensuring that the employee does not work more than their max hours per week
                            fourWeekAverage <= employee.Weeklyworkhours * 60 * 60 && //Ensuring that the employee does not work more than their max hours per week averaged over 4 weeks
                            employee.WorkHoursPerDayInSeconds[routeDay] + routeDuration <= 12 * 60 * 60) //Ensuring that the employee does not work more than 12 hours per day
                        {
                            // Add the route to the employee's list of routes
                            employee.Routes.Add(route);
                            // the routes duration to the employee's total work hours both on week and day
                            employee.WorkhoursWithincurentWeekInSeconds += routeDuration;
                            employee.WorkHoursPerDayInSeconds[routeDay] += routeDuration;

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


        private double CalculateRouteDuration(RouteEntity route)
        {
            // Calculate the total duration of the route based on the assignments
            return route.Assignments.Sum(a => a.Duration);
        }




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------






        //public void GenerateSchema(List<RouteEntity> routes)
        //{

        //    var groups = routes.GroupBy(r => r.RouteDate).Select(group => group.ToList()).ToList();

        //    for (int i = 0; i < groups.Count - 1; i++)
        //    {

        //        // Split routes into two categories based on total time
        //        var splitRoutes = SplitRoutesByTime(groups[i]);

        //        // Get all employees and split them into full-time and part-time
        //        var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
        //        var employeesPartTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours < 40).ToList();

        //        // Assign routes to employees based on their working hours
        //        var result = AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime);

        //        _employeesRepo.AddEmployeesToRoute(result);
        //    }
        //}

        //private List<List<RouteEntity>> SplitRoutesByTime(List<RouteEntity> routes)
        //{
        //    var longRoutes = new List<RouteEntity>();
        //    var shortRoutes = new List<RouteEntity>();

        //    foreach (RouteEntity routeEntity in routes)
        //    {
        //        var totalTime = routeEntity.Assignments.Last().ArrivalTime - routeEntity.Assignments.First().ArrivalTime;
        //        if (totalTime > TimeSpan.FromHours(5))
        //        {
        //            longRoutes.Add(routeEntity);
        //        }
        //        else
        //        {
        //            shortRoutes.Add(routeEntity);
        //        }
        //    }

        //    return new List<List<RouteEntity>> { longRoutes, shortRoutes };
        //}

        //private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime)
        //{
        //    var employeesNeededForTheRoutes = new List<EmployeeEntity>();

        //    AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime, employeesNeededForTheRoutes);

        //    AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime, employeesNeededForTheRoutes);

        //    return employeesNeededForTheRoutes;
        //}

        //private List<EmployeeEntity> AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees, List<EmployeeEntity> employeesNeededForTheRoutes)
        //{
        //    foreach (var employee in employees)
        //    {
        //        if (routes.Count > 0)
        //        {
        //            employee.Route = routes[0];
        //            employeesNeededForTheRoutes.Add(employee);
        //            routes.RemoveAt(0);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    return employeesNeededForTheRoutes;
        //}
        #endregion
    }
}


