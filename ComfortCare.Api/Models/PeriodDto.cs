namespace ComfortCare.Api.Models
{
    /// <summary>
    /// This dto is used to retrieve the statement period that is calculated by the domain logic
    /// </summary>
    public class PeriodDto
    {
        #region Properties
        public int NumberOfDays { get; set; }
        public int NumberOfAssigments { get; set; }
        #endregion
    }
}