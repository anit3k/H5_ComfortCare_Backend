using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComfortCare.Domain.Entities
{
    public class EmployeeStatementPeriodEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int StatementPeriodId { get; set; }
        public int TimeRegistrationId { get; set; }
        public StatementPeriodEntity? StatementPeriod { get; set; } // Opdateret til at bruge den nye klasse
        public TimeRegistrationEntity? TimeRegistration { get; set; } // Opdateret til at bruge den nye klasse
    }

}
