using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.HCNS
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }
        // Basic Information
        public string EmployeeCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string PersonalPhone { get; set; }

        // Address Information
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }

        // Identification
        public string CCCD { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssueDate { get; set; }

        // Work Information
        public string Department { get; set; }
        public string Division { get; set; }
        public string Position { get; set; }

        // Authentication
        public string Username { get; set; }
        public string Password { get; set; } // Consider using secure storage for passwords

        // Contact Information
        public string Email { get; set; }
        public string CompanyPhone { get; set; }

        // Accounting Information
        public string AccountantCode { get; set; }
        public string Extension { get; set; }

        // Tax Information
        public string PersonalTaxCode { get; set; }
        public DateTime TaxIssueDate { get; set; }

        // Employment Dates
        public DateTime JoiningDate { get; set; }
        public DateTime VacationDate { get; set; }
        public DateTime? LeavingDate { get; set; }

        // Work Conditions
        public int WorkRegime { get; set; }
        public int WorkStatus { get; set; }

        // Permissions
        public string JobPermissions { get; set; }

        // Optional ID for NVKD
        public string SalesEmployeeID { get; set; }
        // Avatar (Path or Base64 string)
        public IFormFile Avatar { get; set; }
    }
    public class SelectOption
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    
}

