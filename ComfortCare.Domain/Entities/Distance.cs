namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is used as an entity that hold the distance from one assignment to another,
    /// used in the route generator
    /// </summary>
    public class Distance
    {
        public int AssignmentIdOne { get; set; }
        public int AssignmentIdTwo { get; set; }
        public double DistanceBetween { get; set; }
    }
}
