namespace ComfortCare.Service.Models
{
    /// <summary>
    /// This model is the primary object that contains all the information
    /// for the individual user, about there work schedule
    /// </summary>
    public class EmployeeSchemaModel
    {
        public string Name { get; set; }
        public List<AssignmentSchemaModel> Assignments { get; set; }
    }    
}
