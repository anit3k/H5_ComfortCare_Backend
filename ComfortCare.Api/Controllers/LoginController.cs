using ComfortCare.Api.Models;
using ComfortCare.Service.Interfaces;
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
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public LoginController(IUserService userService)
        {
            _userService = userService;
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
            var loginResult = _userService.ValidateUser(loginDto.Initials, loginDto.Password);

            try
            {
                if (!loginResult)
                {
                    return StatusCode(400, "Bad request, wrong username or password.");
                }
                else
                {
                    var result = _userService.GetEmployeeSchema(loginDto.Initials, loginDto.Password);

                    EmployeeScheduleDto employeeDto = new EmployeeScheduleDto();
                    if (result != null)
                    {
                        employeeDto.Name = result.Name;

                        employeeDto.Assignments = new List<AssignmentDto>();                        

                        foreach (var assignmentData in result.Assignments)
                        {
                            AssignmentDto assignmentDto = new AssignmentDto
                            {
                                Titel = assignmentData.Title,
                                Description = assignmentData.Description,
                                CitizenName = assignmentData.CitizenName,
                                StartDate = assignmentData.StartDate,
                                EndDate = assignmentData.EndDate,
                                Address = assignmentData.Address
                            };
                            employeeDto.Assignments.Add(assignmentDto);
                        }
                    }

                    return StatusCode(200, employeeDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error, please contact your administrator if this continues");
            }
        }
        #endregion
    }
}
