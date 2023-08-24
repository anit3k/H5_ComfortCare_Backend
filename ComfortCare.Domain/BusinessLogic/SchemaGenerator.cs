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

        public void GenerateSchema(List<RouteEntity> routes)
        {

            var groups = routes.GroupBy(r => r.RouteDate).Select(group => group.ToList()).ToList();

            for (int i = 0; i < groups.Count - 1; i++)
            {

                // Split routes into two categories based on total time
                var splitRoutes = SplitRoutesByTime(groups[i]);

                // Get all employees and split them into full-time and part-time
                var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
                var employeesPartTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours < 40).ToList();

                // Assign routes to employees based on their working hours
                var result = AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime);

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

        private List<EmployeeEntity> AssignRoutesToEmployees(List<List<RouteEntity>> splitRoutes, List<EmployeeEntity> employeesFullTime, List<EmployeeEntity> employeesPartTime)
        {
            var employeesNeededForTheRoutes = new List<EmployeeEntity>();

            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime, employeesNeededForTheRoutes);

            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime, employeesNeededForTheRoutes);

            return employeesNeededForTheRoutes;
        }

        private List<EmployeeEntity> AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees, List<EmployeeEntity> employeesNeededForTheRoutes)
        {
            foreach (var employee in employees)
            {
                if (routes.Count > 0)
                {
                    employee.Route = routes[0];
                    employeesNeededForTheRoutes.Add(employee);
                    routes.RemoveAt(0);
                }
            }
            return employeesNeededForTheRoutes;
        }
        #endregion
    }
}


