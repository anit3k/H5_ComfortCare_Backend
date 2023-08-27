namespace ComfortCare.Service.Models
{
    public class AssignmentSchemaModel
    {
        public string Title { get; set; }
        public string AssignmentTypeDescription { get; set; }
        public string Description { get; set; }
        public string CitizenName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
    }
}
