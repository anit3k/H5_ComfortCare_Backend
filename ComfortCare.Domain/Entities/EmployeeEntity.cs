using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComfortCare.Domain.Entities
{
    /// <summary>
    /// This class is used as an entity that hold the Employees for generating Schemas,
    /// </summary>
    public class EmployeeEntity
    {
        public int EmployeeId { get; set; }
        public int Weeklyworkhours { get; set; }
        public int EmployeeType { get; set; }
        public RouteEntity Route { get; set; }
        public double WorkhoursWithincurentWeekInSeconds { get; set; }
        public Queue<double> PastFourWeeksWorkHoursInSeconds { get; set; } = new Queue<double>();
        public Dictionary<DateTime, double> WorkHoursPerDayInSeconds { get; set; } = new Dictionary<DateTime, double>();
        public List<RouteEntity> Routes { get; set; } = new List<RouteEntity>();


    }
}
