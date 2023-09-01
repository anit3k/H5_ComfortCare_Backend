namespace ComfortCare.Data.Models
{
    public class EmployeeRouteDbModel
    {
        public string initials { get; set; }
        public string Name { get; set; }
        public List<EmployeeAssignmentDbModel> Assignments { get; set; }
    }
}
