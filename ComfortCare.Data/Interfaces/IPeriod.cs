using ComfortCare.Domain.Entities;

namespace ComfortCare.Data.Interfaces
{
    public interface IPeriod
    {
        public void CreatePeriod(int days, int numberOfAssignments);

        //public List<EmployeeEntity> CreateEmployeeRoutes(int employeeID);

        //public void WipeAllRoutes();
    }
}
