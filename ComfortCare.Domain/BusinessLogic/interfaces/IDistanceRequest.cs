namespace ComfortCare.Domain.BusinessLogic.interfaces
{
    /// <summary>
    /// This interface is used by the PlanManger to get the distance and timespan on the route between address
    /// </summary>
    public interface IDistanceRequest
    {
        public Task<(double DistanceInSeconds, double DistanceInMeters)> GetDistance(string startLongitude, string startLatitude, string endLongitude, string endLatitude);
    }
}
