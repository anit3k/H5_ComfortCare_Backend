namespace ComfortCare.Api.Models
{
    /// <summary>
    /// This dto is used to retrieve the statement period that should be calculated by the domain logic
    /// It contains only start and end date, along with a validation method to check if the date range is within the
    /// union agreement
    /// </summary>
    public class PeriodDto
    {
        #region Properties
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region Methods
        public bool IsValid()
        {
            TimeSpan dateRange = EndDate - StartDate;
            return dateRange >= TimeSpan.FromDays(1) && dateRange <= TimeSpan.FromDays(7 * 16);
        }
        #endregion
    }
}