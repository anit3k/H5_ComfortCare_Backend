namespace ComfortCare.Data
{
    public class EmployeeSchemaModel
    {
        public string Name { get; set; }
        public List<AssignmentModel> Assignments { get; set; }
    }

    public class AssignmentModel
    {
        public string Title { get; set; }
        public string AssignmentTypeDescription { get; set; }
        public string Description { get; set; }
        public string CitizenName { get; set; }
        public DateTime StartDate { get; set; }
        public string Address { get; set; }
    }
}
