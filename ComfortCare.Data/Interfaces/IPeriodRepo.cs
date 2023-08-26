namespace ComfortCare.Data.Interfaces
{
    public interface IPeriodRepo
    {
        public void CreateNewPeriod(int days, int numberOfAssignments);
    }
}
