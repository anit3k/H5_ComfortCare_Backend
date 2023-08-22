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
        /// <summary>
        /// Checks the request send by the app, with username and password, and validates the inputs.
        /// Also handles errors for no internet.
        /// </summary>
        /// <param name="loginDto">Login data transfer object. Carries the user inputs.</param>
        /// <returns>Returns a status code, and an error message in the body that the app can read on.</returns>
        [HttpPost("Employee")]
        public IActionResult EmployeeLogin(LoginDto loginDto)
        {
            // TODO: get and return schedule for user
            var loginResult = _validator.ValidateUser(loginDto.Initials, loginDto.Password);

            try
            {
                if (loginResult)
                {
                    return StatusCode(200, new EmployeeScheduleDto() { });
                }
                else
                {
                    return StatusCode(400, "Bad request, wrong username or password.");
                } 
            }
            catch (Exception)
            {
                return StatusCode(503, "Service unavailable. No internet available.");
            }
        }
        #endregion

        #region Methods

        #endregion
    }
}
