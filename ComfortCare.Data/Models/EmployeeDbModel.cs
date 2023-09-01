namespace ComfortCare.Data.Models
{
    public class EmployeeDbModel : BaseMongoModel
    {
        public string Initials { get; set; }
        public string EmployeePassword { get; set; }
        public string EmployeeName { get; set; }
        public int WeeklyWorkingHours { get; set; }
        public int EmployeeTypeId { get; set; }
    }
}
