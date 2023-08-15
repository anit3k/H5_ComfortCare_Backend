namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This interface is used by the PlanManger to get the distance and timespan on the route between address
    /// </summary>
    public interface IDistance
    {
        public (float DistanceInSeconds, float DistanceInMeters) GetDistance(string startLongitude, string startLatitude, string endLongitude, string endLatitude);
    }
}
