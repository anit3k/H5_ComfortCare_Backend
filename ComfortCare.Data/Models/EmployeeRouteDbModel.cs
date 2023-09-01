namespace ComfortCare.Data.Models
{
    public class EmployeeRouteDbModel : MongoBaseModel
    {
        public string EmployeeGuid { get; set; }
        public List<Route> Routes { get; set; }
    }

    public class Route 
    {
        public List<string> Assignments { get; set; }
    }

    
}
