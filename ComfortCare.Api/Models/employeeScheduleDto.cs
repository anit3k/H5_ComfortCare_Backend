namespace ComfortCare.Api.Models
{
    /// <summary>
    /// This dto is used for the data/schema to the employee when logging in
    /// </summary>
    public class EmployeeScheduleDto
    {
        #region Properties
        public string Name { get; set; }
        public List<AssignmentDto> Assignments { get; set; }
        #endregion
    }
}
