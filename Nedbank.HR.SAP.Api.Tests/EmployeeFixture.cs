using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Nedbank.HR.SAP.BAL.Constants;
using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.Serivce;
using Nedbank.HR.SAP.Serivce.Controllers;
using Nedbank.HR.SAP.Shared.Interfaces;
using Nedbank.HR.SAP.Shared.Models;

namespace Nedbank.HR.SAP.Api.Test
{
    public class EmployeeFixture
    {
        public Mock<IEmployeeBusiness> EmployeeBusiness { get; }
        public Mock<ILogger<EmployeesController>> Logger { get; }
        public Mock<IDictionary<string, string>> QueryParams {get;}
        
        public EmployeeFixture()
        {
            this.EmployeeBusiness = new Mock<IEmployeeBusiness>();
            this.Logger = new Mock<ILogger<EmployeesController>>();
        }


        public IAugmentedResult<List<Employee>> GetAllEmployees()
        {
            MetaDataResult metaData = null;
            ResultSet resultSet = null;

            var data = this.BulkEmployeeData();
            metaData = new MetaDataResult
            {
                TotalCount = data.Count
            };
            resultSet = new ResultSet
            {
                ResultCode = ResultConstant.SuccessCode,
                ResultDescription = ResultConstant.Success
            };

            AugmentedResult<List<Employee>> augmentedResult = new AugmentedResult<List<Employee>>(resultSet, metaData);
            augmentedResult.Data = data;

            return augmentedResult;
        }


        public IAugmentedResult<Employee> GetEmployeeById(string id)
        {
            MetaDataResult metaData = null;
            ResultSet resultSet = null;

            var data = this.BulkEmployeeData().Find(emp => emp.EmployeeNo.Equals(id));
            metaData = new MetaDataResult
            {
                TotalCount = 1
            };
            resultSet = new ResultSet
            {
                ResultCode = ResultConstant.SuccessCode,
                ResultDescription = ResultConstant.Success
            };

            AugmentedResult<Employee> augmentedResult = new AugmentedResult<Employee>(resultSet, metaData);
            augmentedResult.Data = data;

            return augmentedResult;
        }

        public IAugmentedResult<Employee> UpdateEmployee(List<Employee> bulkData)
        {
            MetaDataResult metaData = null;
            ResultSet resultSet = null;

            
            metaData = new MetaDataResult
            {
                TotalCount = bulkData.Count
            };
            resultSet = new ResultSet
            {
                ResultCode = ResultConstant.SuccessCode,
                ResultDescription = ResultConstant.Success
            };

            AugmentedResult<Employee> augmentedResult = new AugmentedResult<Employee>(resultSet, metaData);
            

            return augmentedResult;
        }
        public IDictionary<string, string> GetQueryParams()
        {
            var query = new Dictionary<string, string>();
            query.Add("employeename", "a");

            return query;
        }

        public List<Employee> BulkEmployeeData()
        {
            var lst = new List<Employee>();
            for (int i = 0; i < 30000; i++)
            {
                var employee = new Employee
                {
                    EmployeeNo = "100" + i,
                    Person = new Person
                    {
                        FirstName = "Firstname" + i,
                        Initials = "",
                        LastName = "Lastname" + i,
                        NickName = ""
                    },
                    Addresses = new Address[]
                    {
                        new Address
                        {
                            AddressLine1 = "Address", SubUrb = "", City = "", PostalCode ="", CountryCode = new CountryCode { Code = "", Description =""}
                        }
                    },
                    Cluster = "Cluster" + (new Random(30)).Next(),
                    ContactDetails = new ContactDetails
                    {
                        ElectronicAddress = new ElectronicAddress[]
                        {
                            new ElectronicAddress
                            { Email ="Email" + i+"@nedbank.co.za", Usage = new Usage { Code = "", Description =""}
                            }
                        },
                        PhoneNumber = new PhoneNumber[]
                        {
                            new PhoneNumber
                            {
                                AreaCode ="",
                                PhoneNo ="+27980808"+i,
                                Usage =  ""
                            }
                        }
                    },
                    EmployeeBranch = new EmployeeBranch
                    {
                        BranchAddress = new Address
                        {
                            AddressLine1 = "",
                        },
                        BranchId = "0000" + i + 1,
                        BranchName = "BranchName" + i,
                        BranchNumber = "CRED00" + new Random(30).Next()
                    },
                    JobTitle = new JobTitle
                    {
                        Code = "JobTitle" + i,
                        Description = "JobTitle" + i
                    },
                    ReportToStaffNo = ""
                };
                lst.Add(employee);
            }

            return lst;
        }
    }
}
