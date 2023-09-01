namespace ComfortCare.Data.Models
{
    public class AssignmentDbModel : BaseMongoModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeFrameStart { get; set; }
        public DateTime TimeFrameEnd { get; set; }
        public double DurationInSeconds { get; set; }
        public int ResidenceId { get; set; }
    }
}
