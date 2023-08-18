namespace ComfortCare.Domain.BusinessLogic
{
    public interface IPlanManager
    {
        void CalculateNewStatementPeriod(DateTime startDate, DateTime endDate);
    }
}