using Bogus;
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
            return Ok(GenerateDummyData());
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method is use to create dummy data in the development phase for the frontend,
        /// it do not get any information from Db, it uses random add static data to simulate the
        /// data need in the view for the users
        /// </summary>
        /// <returns>a data transfer object with dummy content in the development phase</returns>
        private EmployeeScheduleDto GenerateDummyData()
        {
            var result = new EmployeeScheduleDto
            {
                Name = "Fiktiv bruger",
                Assignments = new List<AssignmentDTO>()
            };

            var citizenNames = new[] {
                "Inger Hansen", "Erik Nielsen", "Lise Jensen", "Hans Pedersen", "Karen Møller",
                "Anna Christensen", "Ole Larsen", "Mette Andersen", "Jens Pedersen", "Marie Nielsen",
                "Anders Jensen", "Camilla Olsen", "Thomas Petersen", "Sofie Poulsen", "Michael Hansen"
            };
            var addresses = new[] {
                "Ahornvej 5, 4100 Ringsted", "Bøgevej 12, 4100 Ringsted", "Egevej 8, 4100 Ringsted",
                "Fyrvej 15, 4100 Ringsted", "Granvej 3, 4100 Ringsted", "Solsikkevej 7, 4100 Ringsted",
                "Kastanjevej 14, 4100 Ringsted", "Engvej 22, 4100 Ringsted", "Skovgade 9, 4100 Ringsted",
                "Rosenvej 2, 4100 Ringsted", "Nygade 11, 4100 Ringsted", "Møllegade 6, 4100 Ringsted",
                "Birkevej 18, 4100 Ringsted", "Bakkevej 31, 4100 Ringsted", "Søndre Ringgade 55, 4100 Ringsted"
            };
            var assignmentTitles = new[] {
                "Personlig pleje til ældre borger",
                "Mobilisering og træning",
                "Daglig medicinadministration",
                "Ernæringsvejledning",
                "Sårpleje og bandagering",
                "Assistance ved måltider",
                "Hjemmebesøg og socialt samvær",
                "Indkøb og rengøring",
                "Tilberedning af måltider"
            };

            var lorem = new Bogus.DataSets.Lorem();
            var startDate = DateTime.Now.Date;
            var random = new Randomizer();

            for (int days = 0; days < 7; days++)
            {
                for (int assignmentsPrDay = 0; assignmentsPrDay < 5; assignmentsPrDay++)
                {
                    var assignment = new AssignmentDTO
                    {
                        Titel = assignmentTitles[random.Number(0, assignmentTitles.Length - 1)],
                        Description = lorem.Paragraphs(2),
                        StartDate = startDate.AddHours(random.Number(0, 23)).AddMinutes(random.Number(0, 59)),
                        CitizenName = citizenNames[random.Number(0, citizenNames.Length - 1)],
                        Address = addresses[random.Number(0, addresses.Length - 1)],
                        TimeSpan = random.Number(600, 1800),
                    };

                    assignment.EndDate = assignment.StartDate.AddSeconds(assignment.TimeSpan);

                    result.Assignments.Add(assignment);
                }

                startDate = startDate.AddDays(1);
            }

            return result;
        }
        #endregion
    }
}