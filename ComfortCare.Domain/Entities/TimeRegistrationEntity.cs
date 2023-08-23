using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComfortCare.Domain.Entities
{
    public class TimeRegistrationEntity
    {
        public int Id { get; set; }
        public DateTime DateStartTime { get; set; }
        public float WorkingHours { get; set; }
    }

}
