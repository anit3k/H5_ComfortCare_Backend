using ComfortCare.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComfortCare.Api.Controllers
{
    /// <summary>
    /// This controller is used to make the system calculate a new statement period for all the workers/users
    /// in the ComfortCare ECO system
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodController : ControllerBase
    {
        #region fields

        #endregion

        #region Constructor
        public PeriodController()
        {
            
        }
        #endregion

        #region Controller Methods
        [HttpPost("CalculatePeriod")]
        public IActionResult CalculatePeriod(PeriodDto periodDto) 
        {
            if (!periodDto.IsValid())
            {
                return BadRequest("Invalid date range. The period should be between 1 day and 16 weeks.");
            }
            return Ok(periodDto);
        }
        #endregion

        #region Methods

        #endregion
    }
}
