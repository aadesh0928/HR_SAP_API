using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
namespace Nedbank.HR.SAP.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class HR_SAP_Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Employee number")]
        public string EmployeeNumber { get; set; }
        [BsonElement("Employee Callname")]
        public string EmployeeCallName { get; set; }
        [BsonElement("Employee Firstname")]
        public string EmployeeFirstname { get; set; }
        [BsonElement("Employee Surname")]
        public string EmployeeSurname { get; set; }
        [BsonElement("Employee email address")]
        public string EmployeeEmailAddress { get; set; }
        [BsonElement("Employee cellular number")]
        public string EmployeeCellularNumber { get; set; }
        [BsonElement("Legacy Branch ID")]
        public string LegacyBranchId { get; set; }
        [BsonElement("Employee branch number")]
        public string EmployeeBranchNumber { get; set; }
        [BsonElement("Employee branch name")]
        public string EmployeeBranchName { get; set; }
        [BsonElement("Employee physical location details")]
        public string EmployeePhysicalLocation { get; set; }
        [BsonElement("Employee Office Telephone number")]
        public string EmployeeOfficeTelephone { get; set; }
        [BsonElement("Employee Cluster")]
        public string EmployeeCluster { get; set; }
        [BsonElement("Employee Job Title")]
        public string EmployeeJobTitle { get; set; }
    }
}
