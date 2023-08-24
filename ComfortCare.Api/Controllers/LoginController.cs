using ComfortCare.Api.Models;
using ComfortCare.Data.Interfaces;
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
        private readonly ISchema _schema;
        #endregion

        #region Constructor
        public LoginController(IValidate validator, ISchema schema)
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
                if (!loginResult)
                {
                    return StatusCode(400, "Bad request, wrong username or password.");
                }
                else
                {
                    var result = _schema.CurrentSchema(loginDto.Initials, loginDto.Password);

                    EmployeeScheduleDto employeeDto = new EmployeeScheduleDto();

                    employeeDto.Name = result.Item1;
                    

                    foreach (var item in result.Item2) 
                    {
                        AssignmentDTO assignmentDto = new AssignmentDTO();
                        assignmentDto.Titel = item.AssignmentType.Title;
                        assignmentDto.Description = item.AssignmentType.AssignmentTypeDescription;
                        assignmentDto.StartDate = item.RouteAssignment.;
                        assignmentDto.Address = item.Citizen.Residence.CitizenResidence;
                    }

                    return StatusCode(200, new EmployeeScheduleDto() { });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Service unavailable. No internet available.");
            }
        }
        #endregion

        #region Methods

        #endregion
    }
}
