using ComfortCare.Service.Models;

namespace ComfortCare.Service.Interfaces
{
    public interface IUserService
    {
        public bool ValidateUser(string userName, string password);
        public EmployeeSchemaModel GetEmployeeSchema(string userName, string password);
    }
}
