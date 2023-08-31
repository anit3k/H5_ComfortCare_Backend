namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    /// <summary>
    /// This interface is to separate the boundary between the data and domian layer
    /// </summary>
    public interface IPeriodManager
    {
        public void CalculateNewPeriod(int numberOfDays, int numbersOfAssignments);
    }
}