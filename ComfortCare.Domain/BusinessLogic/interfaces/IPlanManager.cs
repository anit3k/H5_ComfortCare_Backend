namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    public interface IPlanManager
    {
        void CalculateNewStatementPeriod(DateTime startDate, DateTime endDate);
    }
}