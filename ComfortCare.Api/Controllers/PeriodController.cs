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

        [HttpPost("GetRoutesForEmployee")]
        public IActionResult GetRoutesForEmployee(EmployeeDto employeeDto)
        {
            var result = _comfortCareSmartPlanner.CreateEmployeeRoutes(employeeDto.EmployeeID);
            EmployeeScheduleDto employeeScheduleDto = new EmployeeScheduleDto();
            employeeScheduleDto.Name = "john "+employeeDto.EmployeeID;
            employeeScheduleDto.Assignments = new System.Collections.Generic.List<AssignmentDTO>();
            foreach (var item in result)
            {
                AssignmentDTO assignmentDTO = new AssignmentDTO();
                assignmentDTO.Address = "NA";
                assignmentDTO.CitizenName = "NA";
                assignmentDTO.Description = "NA";
                assignmentDTO.EndDate = item.Route.Assignments[0].TimeWindowStart.AddSeconds((int)item.Route.Assignments[0].Duration);
                assignmentDTO.StartDate = item.Route.Assignments[0].TimeWindowStart;
                assignmentDTO.TimeSpan = (int)item.Route.Assignments[0].Duration;
                assignmentDTO.Titel = "NA";
                employeeScheduleDto.Assignments.Add(assignmentDTO);
            }
            return Ok(employeeScheduleDto);
        }

        [HttpPost("WipeAllRoutes")]
        public IActionResult WipeAllRoutes()
        {
            _comfortCareSmartPlanner.WipeAllRoutes();
            return Ok();
        }

        

        #endregion

        #region Methods

        #endregion
    }
}
