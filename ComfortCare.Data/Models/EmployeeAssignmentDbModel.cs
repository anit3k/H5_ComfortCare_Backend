using ComfortCare.Domain.Entities;

namespace ComfortCare.Data.Models
{
    public class EmployeeAssignmentDbModel
    {
        public EmployeeAssignmentDbModel(AssignmentEntity entity, CitizenDbModel citizen, AssignmentDbModel model)
        {
            Title = model.Title;
            Description = model.Description;
            CitizenName = citizen.CitizenName;
            Address = citizen.CitizenResidence;
            StartDate = entity.ArrivalTime;
            EndDate = entity.ArrivalTime.AddSeconds(entity.Duration);
        }

        public string Title { get; set; }
        public string AssignmentTypeDescription { get; set; }
        public string Description { get; set; }
        public string CitizenName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
    }
}
