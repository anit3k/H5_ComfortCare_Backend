namespace ComfortCare.Api.Models
{
    /// <summary>
    /// This dto class is only used in the gateway for mapping data of the assignment to the ui layer
    /// </summary>
    public class AssignmentDto
    {
        #region Properties
        public string Titel { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string CitizenName { get; set; }
        public string Address { get; set; }
        public DateTime EndDate { get; set; }
        #endregion
    }
}
