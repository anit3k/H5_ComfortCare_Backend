﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ComfortCare.Data.Models
{
    public partial class EmployeeStatementPeriod
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int StatementPeriodId { get; set; }
        public int TimeRegistrationId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual StatementPeriod StatementPeriod { get; set; }
        public virtual TimeRegistration TimeRegistration { get; set; }
    }
}