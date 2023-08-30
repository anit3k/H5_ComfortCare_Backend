namespace ComfortCare.Service.Interfaces
{
    /// <summary>
    /// This interface is used in the service layer, and contains the method that
    /// will point to the datalayer
    /// </summary>
    public interface IPeriodService
    {
        public void CreatePeriod(int days, int numberOfAssignment);
    }
}
