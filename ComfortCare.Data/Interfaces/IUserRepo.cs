using ComfortCare.Data.Models;

namespace ComfortCare.Data.Interfaces
{
    /// <summary>
    /// This interface is used for setting the boundary between the service layer and the data layer
    /// </summary>
    public interface IUserRepo
    {
        public bool ValidateUserExist(string username, string password);
        public List<EmployeeRouteDbModel> GetUsersWorkSchedule(string username, string password);
    }
}
