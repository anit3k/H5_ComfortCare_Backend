﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ComfortCare.Data.Models
{
    public partial class Distance
    {
        public int Id { get; set; }
        public int ResidenceOneId { get; set; }
        public int ResidenceTwoId { get; set; }
        public double Duration { get; set; }
        public double DistanceInMeters { get; set; }

        public virtual Residence ResidenceOne { get; set; }
        public virtual Residence ResidenceTwo { get; set; }
    }
}