namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is the entity of a single route that contains a list of assignment for a single day
    /// </summary>
    public class RouteEntity
    {
        #region fields
        private double _totalRouteTimeInHours;
        private int _numberOfAssignments;
        #endregion

        #region Shorthand properties
        public Guid RouteGuid { get; set; }
        public List<AssignmentEntity> Assignments { get; set; }
        public DateTime RouteDate { get; set; }
        #endregion

        #region Full body properties
        public double TotalRouteTimeInHours
        {
            get { return ((Assignments.Last().ArrivalTime.AddSeconds(Assignments.Last().Duration) - Assignments.First().ArrivalTime).TotalHours); }
        }

        public int NumberOfAssignment
        {
            get { return Assignments.Count; }
        }
        #endregion
    }
}
