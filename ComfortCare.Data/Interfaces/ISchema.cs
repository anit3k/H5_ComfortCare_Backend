﻿using ComfortCare.Data.Models;

namespace ComfortCare.Data
{
    /// <summary>
    /// This interface is the used by the controller to get the schema for the active user.
    /// </summary>
    public interface ISchema
    {
        EmployeeRoute CurrentSchema(Employee employee);
    }
}