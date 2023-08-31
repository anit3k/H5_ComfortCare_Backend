using ComfortCare.Service.Models;

namespace ComfortCare.Service.Interfaces
{
    /// <summary>
    /// This interface is used to validate and collect user schemas from the datalayer
    /// </summary>
    public interface IUserService
    {
        public bool ValidateUser(string userName, string password);
        public EmployeeSchemaModel GetEmployeeSchema(string userName, string password);
    }
}
