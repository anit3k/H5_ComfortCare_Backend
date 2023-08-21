using ComfortCare.Api.Models;
using ComfortCare.Domain.BusinessLogic.interfaces;
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
        private readonly IPlanManager _manager;
        #endregion

        #region Constructor
        public PeriodController(IPlanManager manager)
        {
            _manager = manager;
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

            _manager.CalculateNewStatementPeriod(periodDto.StartDate, periodDto.EndDate);

            return Ok(periodDto);
        }               
        #endregion

        #region Methods

        #endregion
    }
}
