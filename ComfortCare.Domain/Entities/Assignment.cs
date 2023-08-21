namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is used as an entity to hold the assignmets that is not yet been assigned to an employee
    /// </summary>
    public class Assignment
    {
        public int Id { get; set; }
        public DateTime TimeWindowStart{ get; set; }
        public DateTime TimeWindowEnd { get; set; }
        public double Duration { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
