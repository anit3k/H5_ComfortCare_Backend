namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is used as an entity that hold the distance from one assignment to another,
    /// used in the route generator
    /// </summary>
    public class DistanceEntity
    {
        public int AssignmentOne { get; set; }
        public int AssignmentTwo { get; set; }
        public double DistanceBetween { get; set; }
    }
}
