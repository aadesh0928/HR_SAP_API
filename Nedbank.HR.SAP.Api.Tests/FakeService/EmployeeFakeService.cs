using Nedbank.HR.SAP.BAL.Constants;
using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.Shared.Interfaces;
using Nedbank.HR.SAP.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.Api.Test.FakeService
{
    public class EmployeeFakeService : IEmployeeBusiness
    {
        public Task<IAugmentedResult<Employee>> GetEmployeeByIdAsync(IDictionary<string, string> pathParam)
        {
            var employeeId = pathParam["employeenumber"];
            var data = this.BulkEmployeeData().Find(employee => employee.EmployeeNo.Equals(employeeId, StringComparison.InvariantCultureIgnoreCase));
            var metaData = new MetaDataResult
            {
                TotalCount = 1
            };
            var resultSet = new ResultSet
            {
                ResultCode = ResultConstant.SuccessCode,
                ResultDescription = ResultConstant.Success
            };
            var augmentedResult = new AugmentedResult<Employee>(resultSet, metaData);
            augmentedResult.Data = data;

            return Task.FromResult<IAugmentedResult<Employee>>(augmentedResult);
        }

        public Task<IAugmentedResult<List<Employee>>> GetEmployeesAsync(IDictionary<string, string> queryParams)
        {
            MetaDataResult metaData = null;
            ResultSet resultSet = null;

            AugmentedResult<List<Employee>> augmentedResult = new AugmentedResult<List<Employee>>(resultSet, metaData);

            if (queryParams.Count == 0)
            {
                resultSet.ResultCode = ResultConstant.BadRequestCode;
                resultSet.ResultDescription = ResultConstant.BadRequest;
            }
            else
            {
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
                augmentedResult.Data = data;
                augmentedResult.ResultSet = resultSet;
                augmentedResult.MetaData = metaData;
            }



            return Task.FromResult<IAugmentedResult<List<Employee>>>(augmentedResult);
        }

        public Task<IAugmentedResult<Employee>> UpdateEmployeesAsync(List<Employee> employees)
        {
            throw new NotImplementedException();
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
