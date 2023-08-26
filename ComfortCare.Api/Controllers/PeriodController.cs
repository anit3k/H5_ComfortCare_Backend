using ComfortCare.Api.Models;
using ComfortCare.Data.Interfaces;
using ComfortCare.Service.Interfaces;
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
        private readonly IPeriodService _periodService;
        #endregion

        #region Constructor
        public PeriodController(IPeriodService periodService)
        {
            _periodService = periodService;
        }
        #endregion

        #region Controller Methods
        [HttpPost("CalculatePeriod")]
        public IActionResult CalculatePeriod(PeriodDto periodDto) 
        {
            _periodService.CreatePeriod(periodDto.NumberOfDays, periodDto.NumberOfAssigments);
            return Ok();
        }
        #endregion
    }
}
