﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ComfortCare.Data.Models
{
    public partial class RouteAssignment
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int EmployeeRouteId { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual EmployeeRoute EmployeeRoute { get; set; }
    }
}