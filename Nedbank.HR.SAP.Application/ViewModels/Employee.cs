using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nedbank.HR.SAP.BAL.ViewModels
{
    public class Employee: IEquatable<Employee>
    {
        public string EmployeeNo { get; set; }
        public Person Person { get; set; }
        public ContactDetails ContactDetails { get; set; }
        public Address[] Addresses { get; set; }
        public JobTitle JobTitle { get; set; }
        public string ReportToStaffNo { get; set; }
        public string Cluster { get; set; }
        public EmployeeBranch EmployeeBranch { get; set; }

        public bool Equals([AllowNull] Employee other)
        {
            return !string.IsNullOrEmpty(EmployeeNo) && EmployeeNo.Equals(other.EmployeeNo);
        }
        public override int GetHashCode()
        {
            return EmployeeNo.GetHashCode();
        }
    }

    public class JobTitle
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Person
    {
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
    }

    public class ContactDetails
    {
        public ElectronicAddress[] ElectronicAddress { get; set; }
        public PhoneNumber[] PhoneNumber { get; set; }
    }

    public class ElectronicAddress
    {
        public string Email { get; set; }
        public Usage Usage { get; set; }
    }
    public class Usage
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class PhoneNumber
    {
        public string AreaCode { get; set; }
        public string PhoneNo { get; set; }
        public string Usage { get; set; }
    }

    public class Address
    {
        public AddressType AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string SubUrb { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public CountryCode CountryCode { get; set; }
    }

    public class AddressType
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class CountryCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeBranch
    {
        public string BranchId { get; set; }
        public string BranchNumber { get; set; }
        public string BranchName { get; set; }
        public Address BranchAddress { get; set; }

    }

    public class BranchAddress
    {
        public AddressType AddressType { get; set; }
    }

    public class UserSessionInfo
    {
        public string OperatingBranch { get; set; }
        public Role[] Roles { get; set; }
        public Role CurrentRole { get; set; }
    }

    public class Role
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
