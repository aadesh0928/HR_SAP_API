using System;

namespace Nedbank.HR.SAP.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string EmployeeCellularNumber { get; set; }
        public string EmployeeOfficeTelephoneNumber { get; set; }
        public string EmployeeBranchID { get; set; }
        public string EmployeeBranchNumber { get; set; }
        public string EmployeeBranchName { get; set; }
        public string EmployeePhysicalLocationFloor { get; set; }
        public string EmployeePhysicalLocationBuildingName { get; set; }
        public string EmployeePhysicalLocationStreetNumber { get; set; }
        public string EmployeePhysicalLocationAddress { get; set; }
        public string EmployeePhysicalLocationSuburb { get; set; }
        public string EmployeePhysicalLocationCity { get; set; }
        public string EmployeePhysicalLocationProvince { get; set; }
        public string EmployeePhysicalLocationCountry { get; set; }
        public string EmployeeNickName { get; set; }
        public string EmployeeJobTitle { get; set; }
        public string EmployeeCluster { get; set; }

    }
}
