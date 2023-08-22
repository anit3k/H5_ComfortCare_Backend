using ComfortCare.Api.Models;
using ComfortCare.Data;
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
        private readonly IGetStatementPeriod _comfortCareSmartPlanner;
        #endregion

        #region Constructor
        public PeriodController(IGetStatementPeriod smartPlanner)
        {
            _comfortCareSmartPlanner = smartPlanner;
        }
        #endregion

        #region Controller Methods
        [HttpPost("CalculatePeriod")]
        public IActionResult CalculatePeriod(PeriodDto periodDto) 
        {
            var result = _comfortCareSmartPlanner.CreateStatementPeriod(periodDto.NumberOfDays, periodDto.NumberOfAssigments);
            return Ok(result);
        }               
        #endregion

        #region Methods

        #endregion
    }
}
