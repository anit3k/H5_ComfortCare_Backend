using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;
using System.Globalization;

namespace ComfortCare.Domain.BusinessLogic
{

    /// <summary>
    /// This class is used to populate routes with employees, it should use the rules for workinghours
    /// and look at the induvidual employee to calculate their specific working time etc...
    /// </summary>
    public class SchemaGenerator
    {


        #region Fields
        private readonly IEmployeesRepo _employeesRepo;
        private readonly IRouteConstructionRepo _routeRepo;
        #endregion




        #region Constructor 
        public SchemaGenerator(IEmployeesRepo employeesRepo, IRouteConstructionRepo routeRepo)
        {
            _employeesRepo = employeesRepo;
            _routeRepo = routeRepo;
        }
        #endregion









        //TODO: Kent, add logic to ensure that it has been at least 11 hours ago since employy has been working
        //TODO: Kent - add logic to check for employees who have not worked within other timespans for 48 hours within this period
        #region Methods


        public List<EmployeeEntity> GenerateSchema(List<RouteEntity> routes)
        {
            // Split routes into two categories based on total time
            var splitRoutes = SplitRoutesByTime(routes);

            // Get all employees and split them into full-time and part-time
            var employeesFullTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours == 40).ToList();
            var employeesPartTime = _employeesRepo.GetAllEmployees().Where(e => e.Weeklyworkhours < 40).ToList();

            // Assign routes to employees based on their working hours
            return AssignRoutesToEmployees(splitRoutes, employeesFullTime, employeesPartTime);
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

            // Assign long routes to full-time employees
            AssignRoutesToSpecificEmployees(splitRoutes[0], employeesFullTime, employeesNeededForTheRoutes);

            // Assign short routes to part-time employees
            AssignRoutesToSpecificEmployees(splitRoutes[1], employeesPartTime, employeesNeededForTheRoutes);

            return employeesNeededForTheRoutes;
        }

        private void AssignRoutesToSpecificEmployees(List<RouteEntity> routes, List<EmployeeEntity> employees, List<EmployeeEntity> employeesNeededForTheRoutes)
        {
            foreach (var employee in employees)
            {
                if (routes.Count > 0)
                {
                    employee.Route = routes[0];
                    routes.RemoveAt(0);
                }
                employeesNeededForTheRoutes.Add(employee);
            }
        }


        //public List<EmployeeEntity> GenerateSchema(List<RouteEntity> rutes)
        //{
        //    List<RouteEntity> tempRutes = new List<RouteEntity>(rutes);

        //    List<List<RouteEntity>> splitRoutes = new List<List<RouteEntity>>();
        //    splitRoutes.Add(new List<RouteEntity>());
        //    splitRoutes.Add(new List<RouteEntity>());

        //    foreach (RouteEntity routeEntity in tempRutes)
        //    {
        //        var start = routeEntity.Assignments[0].ArrivalTime;
        //        var end = routeEntity.Assignments[routeEntity.Assignments.Count - 1].ArrivalTime;
        //        var totaltime = end - start;
        //        if (totaltime > TimeSpan.FromHours(5))
        //        {
        //            splitRoutes[0].Add(routeEntity);
        //        }
        //        else
        //        {
        //            splitRoutes[1].Add(routeEntity);
        //        }
        //    }


        //    List<EmployeeEntity> employees = _employeesRepo.GetAllEmployees();
        //    List<EmployeeEntity> employeesFullTime = employees.Where(e => e.Weeklyworkhours == 40).ToList();
        //    List<EmployeeEntity> employeesPartTime = employees.Where(e => e.Weeklyworkhours < 40).ToList();
        //    List<EmployeeEntity> employeesNeededForTheRoutes = new List<EmployeeEntity>();
        //    foreach (var employee in employeesFullTime)
        //    {
        //        if (splitRoutes[0].Count > 0)
        //        {
        //            employee.Route = splitRoutes[0][0];
        //            splitRoutes[0].RemoveAt(0);
        //        }
        //        employeesNeededForTheRoutes.Add(employee);
        //    }
        //    foreach (var employee in employeesPartTime)
        //    {
        //        if (splitRoutes[1].Count > 0)
        //        {
        //            employee.Route = splitRoutes[1][0];
        //            splitRoutes[1].RemoveAt(0);
        //        }
        //        employeesNeededForTheRoutes.Add(employee);
        //    }
        //    return employeesNeededForTheRoutes;
        //}


        public void SaveSchema(List<EmployeeEntity> employees)
        {
            _employeesRepo.InsertRoutes(employees);
        }



        public List<EmployeeEntity> GetRoutesForCurrentEmployee(int employeeId)
        {
            return _employeesRepo.GetRoutesForCurrentEmployee(employeeId);
        }


        public void WipeAllRoutes()
        {
            _employeesRepo.WipeAllRoutes();
        }






        #endregion
    }
}


