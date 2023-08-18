namespace ComfortCare.Api.Models
{
    /// <summary>
    /// This dto is used for the data/schema to the employee when logging in
    /// </summary>
    public class EmployeeScheduleDto
    {
        #region Properties
        public string Name { get; set; }
        public List<AssignmentDTO> Assignments { get; set; }
        #endregion
    }

    public class AssignmentDTO
    {
        #region Properties
        public string Titel { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string CitizenName { get; set; }
        public string Address { get; set; }
        public int TimeSpan { get; set; }
        public DateTime EndDate { get; set; }
        #endregion
    }
}
