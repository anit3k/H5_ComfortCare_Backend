namespace ComfortCare.Data.Interfaces
{
    /// <summary>
    /// This interface is lives in the data layer and is used as the entry point of
    /// the service layer, to set the boundary between service and data layer.
    /// this is also some kind of mediator for the separations of layers
    /// </summary>
    public interface IPeriodRepo
    {
        public void CreateNewPeriod(int days, int numberOfAssignments);
    }
}
