namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is the entity of a single route that contains a list of assignment for a single day
    /// </summary>
    public class Route
    {
        // TODO: Do we need an Id here!??!!?!?
        public Guid RouteId { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
