using ComfortCare.Domain.Entities;

namespace ComfortCare.Data.Models
{
    public class EmployeeRouteDbModel : MongoBaseModel
    {
        public EmployeeRouteDbModel(EmployeeDbModel model)
        {
            Initials = model.Initials;
            Name = model.EmployeeName;
            Assignments = new List<EmployeeAssignmentDbModel>();
        }
        public string Initials { get; set; }
        public string Name { get; set; }
        public List<EmployeeAssignmentDbModel> Assignments { get; set; }
    }
}
