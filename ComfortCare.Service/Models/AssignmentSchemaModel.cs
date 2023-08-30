namespace ComfortCare.Service.Models
{
    /// <summary>
    /// This model class is used to map data from the datalayer into an kombined object
    /// that contains all the needed information for the user to see in the UI layer
    /// </summary>
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
