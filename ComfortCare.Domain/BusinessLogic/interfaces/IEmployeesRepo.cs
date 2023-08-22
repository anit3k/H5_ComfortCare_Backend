using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    public interface IEmployeesRepo
    {
        public List<EmployeeEntity> GetAllEmployees();
    }
}
