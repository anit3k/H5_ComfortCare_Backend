using ComfortCare.Data.Models;

namespace ComfortCare.Data.Interfaces
{
    public interface IUserRepo
    {
        public bool ValidateUserExist(string username, string password);
        public List<EmployeeRouteDbModel> GetUsersWorkSchedule(string username, string password);
    }
}
