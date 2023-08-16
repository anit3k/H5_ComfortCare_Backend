using ComfortCare.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComfortCare.Api.Controllers
{
    /// <summary>
    /// This controller is used for testing purposes, only in the development phase, to make it 
    /// possible to used the back and frontend together while the login controller is being created
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        #region fields
        #endregion

        #region Constructor
        public TestController()
        {

        }
        #endregion

        #region Controller Methods

        /// <summary>
        /// This controller method is used to sent static test data to our 
        /// frontend flutter application
        /// </summary>
        /// <returns></returns>
        [HttpPost("LoginTestEmployee")]
        public IActionResult LoginTestEmployee(LoginDto loginDto)
        {
            // TODO: return static list with schema, for development and testing only!
            return Ok(new employeeScheduleDto() { Schema = "This is your schema!" });
        }
        #endregion

        #region Methods
        #endregion
    }
}