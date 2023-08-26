using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;
using ComfortCare.Service.Interfaces;

namespace ComfortCare.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userService)
        {
            _userRepo = userService;
        }
        public EmployeeSchemaModel GetEmployeeSchema(string userName, string password)
        {
            return _userRepo.GetUsersWorkSchedule(userName, password);
        }

        public bool ValidateUser(string userName, string password)
        {
            return _userRepo.ValidateUserExist(userName, password);
        }
    }
}
