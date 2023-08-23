using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    public interface IEmployeesRepo
    {
        public List<EmployeeEntity> GetAllEmployees();
        public List<EmployeeStatementPeriodEntity> GetEmployeeStatementPeriodsWithDetails();
        public void InsertRoutes(List<EmployeeEntity> employees);

        public List<EmployeeEntity> GetRoutesForCurrentEmployee(int employeeID);
        public void WipeAllRoutes();
    }
}
