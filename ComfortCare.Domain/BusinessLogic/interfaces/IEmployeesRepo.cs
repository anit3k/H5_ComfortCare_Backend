using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    /// <summary>
    /// This interface is used to by the schema generator to collect all the employees
    /// and save all the route and assignments added for each employee
    /// </summary>
    public interface IEmployeesRepo
    {
        public List<EmployeeEntity> GetAllEmployees();
        public void AddEmployeesToRoute(List<EmployeeEntity> employees);
    }
}
