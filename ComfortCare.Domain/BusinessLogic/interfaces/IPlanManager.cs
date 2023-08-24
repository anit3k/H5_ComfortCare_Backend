using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    public interface IPlanManager
    {
        public void CalculateNewPeriod(int numberOfDays, int numbersOfAssignments);

        //public List<EmployeeEntity> GetEmployeeRoutes(int employeeID);

        //public void WipeAllRoutes();
    }
}