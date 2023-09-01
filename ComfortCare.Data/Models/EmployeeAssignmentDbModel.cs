namespace ComfortCare.Data.Models
{
    public class EmployeeAssignmentDbModel
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
