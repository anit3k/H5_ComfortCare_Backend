namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is the entity of a single route that contains a list of assignment for a single day
    /// </summary>
    public class RouteEntity
    {
        // TODO: Do we need an Id here!??!!?!?
        public Guid RouteGuid { get; set; }
        public List<AssignmentEntity> Assignments { get; set; }
    }
}
