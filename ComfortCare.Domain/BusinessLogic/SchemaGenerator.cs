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

        public List<EmployeeEntity> GenerateSchema(List<RouteEntity> rutes)
        {
            List<RouteEntity> tempRutes = new List<RouteEntity>(rutes);

            List<List<RouteEntity>> splitRoutes = new List<List<RouteEntity>>();
            splitRoutes.Add(new List<RouteEntity>());
            splitRoutes.Add(new List<RouteEntity>());

            foreach (RouteEntity routeEntity in tempRutes)
            {
                var start = routeEntity.Assignments[0].ArrivalTime;
                var end = routeEntity.Assignments[routeEntity.Assignments.Count - 1].ArrivalTime;
                var totaltime = end - start;
                if (totaltime < TimeSpan.FromHours(5))
                {
                    splitRoutes[0].Add(routeEntity);
                }
                else
                {
                    splitRoutes[1].Add(routeEntity);
                }
            }


            List<EmployeeEntity> employees = _employeesRepo.GetAllEmployees();
            //Add a route to each employee as long as there are routes left
            foreach (var employee in employees)
            {
                if (tempRutes.Count > 0)
                {
                    employee.Route = tempRutes[0];
                    tempRutes.RemoveAt(0);
                }
            }
            return employees;
        }


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


