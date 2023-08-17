using ComfortCare.Api.Models;
using ComfortCare.Data;
using Microsoft.AspNetCore.Mvc;

namespace ComfortCare.Api.Controllers
{
    /// <summary>
    /// This is the login controller, this is the only entry point in production 
    /// for the employees/users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region fields
        private readonly IValidate _validator;
        private readonly IGetSchema _schema;
        #endregion

        #region Constructor
        public LoginController(IValidate validator, IGetSchema schema)
        {
            _validator = validator;
            _schema = schema;
        }
        #endregion

        #region Controller Methods
        [HttpPost("Employee")]
        public IActionResult EmployeeLogin(LoginDto loginDto)
        {
            // TODO: make validation on user
            // TODO: get and return schedule for user

            return Ok(new employeeScheduleDto() { Schema = "This is your schema!"});
        }
        #endregion

        #region Methods

        #endregion
    }
}
