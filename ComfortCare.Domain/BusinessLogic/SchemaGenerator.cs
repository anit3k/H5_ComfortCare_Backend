using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

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
        #endregion




        #region Constructor 
        public SchemaGenerator(IEmployeesRepo employeesRepo)
        {
            _employeesRepo = employeesRepo;
        }
        #endregion



        //TODO: Kent, add logic to ensure that it has been at least 11 hours ago since employy has been working
        #region Methods
        public void GenerateSchema(List<RouteEntity> rutes)
        {
            //TODO: Kent - add logic to check for employees who have not worked within other timespans for 48 hours within this period


            var employees = _employeesRepo.GetAllEmployees();
            var test = 0;
        }
        #endregion
    }
}


