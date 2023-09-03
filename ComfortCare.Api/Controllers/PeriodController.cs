using ComfortCare.Api.Models;
using ComfortCare.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("---- Enter Period Controller -----");
#endif
            _periodService.CreatePeriod(periodDto.NumberOfDays, periodDto.NumberOfAssigments);

#if DEBUG
            stopwatch.Stop(); // Stop the stopwatch
            double elapsedMinutes = stopwatch.Elapsed.TotalMinutes;
            Console.WriteLine($"---- Periode controlller exit at the time: {elapsedMinutes} ----");
#endif
            return Ok();
        }
        #endregion
    }
}
