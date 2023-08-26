namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    public interface IPeriodManager
    {
        public void CalculateNewPeriod(int numberOfDays, int numbersOfAssignments);
    }
}