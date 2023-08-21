namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is used as an entity that hold the distance from one assignment to another,
    /// used in the route generator
    /// </summary>
    public class DistanceEntity
    {
        public AssignmentEntity AssignmentOne { get; set; }
        public AssignmentEntity AssignmentTwo { get; set; }
        public double DistanceBetween { get; set; }
    }
}
