using AutoMapper;
using Microsoft.Extensions.Logging;
using Nedbank.HR.SAP.BAL.Constants;
using Nedbank.HR.SAP.BAL.Enums;
using Nedbank.HR.SAP.BAL.Filters;
using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.DAL.Interfaces;
using Nedbank.HR.SAP.DAL.Models;
using Nedbank.HR.SAP.Shared.Interfaces;
using Nedbank.HR.SAP.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.BAL.Business
{
    public class EmployeeBusiness : IEmployeeBusiness
    {

        #region Fields


        private readonly IAugmentedResult<List<Employee>> _augmentedEmployeesList;
        private readonly IAugmentedResult<Employee> _augmentedEmployee;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IExpressionFilter<HR_SAP_Employee> _expressionFilter;
        private readonly ISearchFilterSettings _searchFilterSettings;
        private readonly IBulkUpdateSettings _bulkUpdateSettings;
       
        private IDictionary<string, string> _filterParams { get; set; }
        private List<Employee> _validatedEmployeeList { get; set; }
        private List<Employee> _distinctEmployeeList { get; set; }
        private long _bulkUpdateRecordCount { get; set; }
        private readonly ILogger<EmployeeBusiness> _logger;

        #endregion

        #region Ctor
        public EmployeeBusiness(IAugmentedResult<List<Employee>> augmentedEmployeesList,
            IAugmentedResult<Employee> augmentedEmployee,
            IEmployeeRepository employeeRepository, IMapper mapper,
            ISearchFilterSettings searchFilterSettings,
            IBulkUpdateSettings bulkUpdateSettings,
            IExpressionFilter<HR_SAP_Employee> expressionFilter,
            ILogger<EmployeeBusiness> logger)
        {
            _augmentedEmployeesList = augmentedEmployeesList;
            _augmentedEmployee = augmentedEmployee;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _searchFilterSettings = searchFilterSettings;
            _bulkUpdateSettings = bulkUpdateSettings;
            _expressionFilter = expressionFilter;
            _logger = logger;
        }


        #endregion

        #region Methods
        /// <summary>
        /// My function
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<IAugmentedResult<List<Employee>>> GetEmployeesAsync(IDictionary<string, string> queryParams)
        {
            try
            {
                _logger.LogInformation("Call initiated for GetEmployeesAsync");
                if(queryParams.Count == 0)
                {
                    _logger.LogInformation("No query param supplied for GetEmployeesAsync");
                    _augmentedEmployeesList.ResultSet.ResultCode = ResultConstant.BadRequestCode;
                    _augmentedEmployeesList.ResultSet.ResultDescription = ResultConstant.BadRequest;
                    return _augmentedEmployeesList;
                }

                _filterParams = queryParams;

                var employees = await FetchEmployeesAsync();

                if (employees.Count == 0)
                {
                    _logger.LogInformation("No employee retrieved");
                    _augmentedEmployeesList.ResultSet.ResultCode = ResultConstant.NotFoundCode;
                    _augmentedEmployeesList.ResultSet.ResultDescription = ResultConstant.NotFound;
                }
                else
                {
                    _logger.LogInformation($"Employees read count {employees.Count}");
                    var employeesViewModel = _mapper.Map<List<Employee>>(employees);
                    _augmentedEmployeesList.Data = employeesViewModel;
                    _augmentedEmployeesList.MetaData.TotalCount = employeesViewModel.Count;
                    _augmentedEmployeesList.ResultSet.ResultCode = ResultConstant.SuccessCode;
                    _augmentedEmployeesList.ResultSet.ResultDescription = ResultConstant.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception in GetEmployeesAsync :: ", ex);
                throw;
            }
            _logger.LogInformation("Call completed for GetEmployeesAsync");
            return _augmentedEmployeesList;
        }

        public async Task<IAugmentedResult<Employee>> GetEmployeeByIdAsync(IDictionary<string, string> pathParam)
        {
            try
            {
                _logger.LogInformation("Call initiated for GetEmployeeByIdAsync");
                _filterParams = pathParam;

                var employees = await FetchEmployeesAsync();

                if (employees.FirstOrDefault() == null)
                {
                    _logger.LogInformation($"No employee retrieved for employee id : {pathParam["employeenumber"]}");
                    _augmentedEmployee.ResultSet.ResultCode = ResultConstant.NotFoundCode;
                    _augmentedEmployee.ResultSet.ResultDescription = ResultConstant.NotFound;
                }
                else
                {
                    _logger.LogInformation($"Employee found for employee id : {pathParam["employeenumber"]}");
                    var employeesViewModel = _mapper.Map<Employee>(employees.FirstOrDefault());
                    _augmentedEmployee.Data = employeesViewModel;
                    _augmentedEmployee.ResultSet.ResultCode = ResultConstant.SuccessCode;
                    _augmentedEmployee.ResultSet.ResultDescription = ResultConstant.Success;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception in GetEmployeeByIdAsync :: ", ex);
                throw;
            }

            _logger.LogInformation("Call completed for GetEmployeeByIdAsync");
            return _augmentedEmployee;
        }

        public async Task<IAugmentedResult<Employee>> UpdateEmployeesAsync(List<Employee> employees)
        {
            try
            {
                _logger.LogInformation("Call initiated for UpdateEmployeesAsync");

                _distinctEmployeeList = employees.Distinct().ToList();

                ValidateEmployees();
                
                FilterEmployeesValidation();

                if (_validatedEmployeeList.Count != 0)
                {
                    var list_sap_employees = _mapper.Map<List<HR_SAP_Employee>>(_validatedEmployeeList);
                    _bulkUpdateRecordCount = await _employeeRepository.BulkUpdateEmployeeAsync(list_sap_employees);
                }

                PrepareUpdateResultSet();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception in UpdateEmployeesAsync :: ", ex);
                throw;
            }

            _logger.LogInformation("Call completed for UpdateEmployeesAsync");

            return _augmentedEmployee;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Function to fetch employees after creating filter statements
        /// and calling the repository with the expression filters.
        /// </summary>
        /// <returns></returns>
        private async Task<List<HR_SAP_Employee>> FetchEmployeesAsync()
        {
            CreateFilterStatements();
            return await _employeeRepository.FetchEmployeesAsync(_expressionFilter.BuildExpression());
        }

        /// <summary>
        /// Functions creates the filter statements
        /// for expression filter based on the received 
        /// params from the API methods, and  searchfilterSettings
        /// </summary>
        private void CreateFilterStatements()
        {
            try
            {
                var searchFilterSettings = _searchFilterSettings.Values;

                _filterParams.ToList().ForEach(queryParam =>
                {
                    if (searchFilterSettings.ContainsKey(queryParam.Key.ToLower()))
                    {
                        var parameterValue = queryParam.Value;
                        if (parameterValue != null && !string.IsNullOrEmpty(parameterValue))
                        {
                            var filterStatement = new FilterStatement
                            {
                                FilterOperation = (FilterOperation)Enum.Parse(typeof(FilterOperation), searchFilterSettings[queryParam.Key.ToLower()][FilterConstant.Operator]),
                                PropertyName = searchFilterSettings[queryParam.Key.ToLower()][FilterConstant.MongoFieldName],
                                Value = parameterValue
                            };
                            _expressionFilter.Statements.Add(filterStatement);
                        }
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function validates the distinct employees for the
        /// properties validations for _distinctEmployeeList object.
        /// </summary>
        /// <returns></returns>
        private void ValidateEmployees()
        {
            try
            {

                _distinctEmployeeList.ForEach(employee =>
                {

                    ValidationItem validationItem = null;
                    var listValidations = new List<ValidationItem>();
                    if (string.IsNullOrEmpty(employee.EmployeeNo))
                    {
                        validationItem = new ValidationItem(nameof(employee.EmployeeNo), "Employee number cannot be empty", employee.EmployeeNo);
                        listValidations.Add(validationItem);
                    }
                    if (employee.Person == null)
                    {
                        validationItem = new ValidationItem(nameof(employee.Person), "Employee's person details are missing", employee.Person);
                        listValidations.Add(validationItem);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(employee.Person.FirstName))
                        {
                            validationItem = new ValidationItem(nameof(employee.Person.FirstName), "Employee first name cannot be empty", employee.Person.FirstName);
                            listValidations.Add(validationItem);
                        }
                        if (string.IsNullOrEmpty(employee.Person.LastName))
                        {
                            validationItem = new ValidationItem(nameof(employee.Person.LastName), "Employee last name cannot be empty", employee.Person.LastName);
                            listValidations.Add(validationItem);
                        }
                    }

                    if (employee.EmployeeBranch == null)
                    {
                        validationItem = new ValidationItem(nameof(employee.EmployeeBranch), "Employee's branch details are missing", employee.EmployeeBranch);
                        listValidations.Add(validationItem);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(employee.EmployeeBranch.BranchId))
                        {
                            validationItem = new ValidationItem(nameof(employee.EmployeeBranch.BranchId), "Employee branch id cannot be empty", employee.EmployeeBranch.BranchId);
                            listValidations.Add(validationItem);
                        }
                        else
                        {
                            if(employee.EmployeeBranch.BranchId.Length != 4)
                            {
                                validationItem = new ValidationItem(nameof(employee.EmployeeBranch.BranchId), "Employee brnach needs to be 4 digit number", employee.EmployeeBranch.BranchId);
                                listValidations.Add(validationItem);
                            }
                        }
                        if (string.IsNullOrEmpty(employee.EmployeeBranch.BranchName))
                        {
                            validationItem = new ValidationItem(nameof(employee.EmployeeBranch.BranchName), "Employee branch name cannot be empty", employee.EmployeeBranch.BranchName);
                            listValidations.Add(validationItem);
                        }
                        if (string.IsNullOrEmpty(employee.EmployeeBranch.BranchNumber))
                        {
                            validationItem = new ValidationItem(nameof(employee.EmployeeBranch.BranchNumber), "Employee branch number cannot be empty", employee.EmployeeBranch.BranchNumber);
                            listValidations.Add(validationItem);
                        }
                    }

                    if (employee.JobTitle == null)
                    {
                        validationItem = new ValidationItem(nameof(employee.JobTitle), "Employee's job title details are missing", employee.JobTitle);
                        listValidations.Add(validationItem);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(employee.JobTitle.Code))
                        {
                            validationItem = new ValidationItem(nameof(employee.JobTitle.Code), "Employee Job title code cannot be empty", employee.JobTitle.Code);
                            listValidations.Add(validationItem);
                        }
                    }

                    if (employee.ContactDetails == null)
                    {
                        validationItem = new ValidationItem(nameof(employee.ContactDetails), "Employee's contact details are missing", employee.ContactDetails);
                        listValidations.Add(validationItem);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(employee.ContactDetails.PhoneNumber.First().PhoneNo))
                        {
                            validationItem = new ValidationItem(nameof(employee.ContactDetails.PhoneNumber), "Employee phone number cannot be empty", employee.ContactDetails.PhoneNumber.First().PhoneNo);
                            listValidations.Add(validationItem);
                        }
                    }


                    if (listValidations.Count != 0)
                    {
                        var validationResult = new ValidationResult<Employee>();
                        validationResult.DataItem = employee;
                        validationResult.Validations = listValidations;

                        _augmentedEmployee.ValidationResult.Add(validationResult);
                        validationResult = null;
                    }
                    else
                    {
                        listValidations = null;
                    }

                });

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Filters the distinct employees list for the validations
        /// </summary>
        /// <returns></returns>
        private void FilterEmployeesValidation()
        {
            try
            {
                _validatedEmployeeList = _distinctEmployeeList
                            .Except(_augmentedEmployee.ValidationResult.Select(emp => emp.DataItem)).ToList();

                // capping the valiations results as per the settings
                _augmentedEmployee.ValidationResult = _augmentedEmployee.ValidationResult
                            .Take(_bulkUpdateSettings.ValidationRecordsCapping).ToList();
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This functions prepares the resultset object or the bulk update employees
        /// operation 
        /// </summary>
        private void PrepareUpdateResultSet()
        {
            if(_validatedEmployeeList.Count == 0)
            {
                _augmentedEmployee.ResultSet.ResultCode = ResultConstant.FailedCode;
                _augmentedEmployee.MetaData.TotalCount = 0;
                _augmentedEmployee.ResultSet.ResultDescription = ResultConstant.Failed;
            }
            else if(_validatedEmployeeList.Count == _distinctEmployeeList.Count)
            {
                _augmentedEmployee.ResultSet.ResultCode = ResultConstant.SuccessCode;
                _augmentedEmployee.ResultSet.ResultDescription = ResultConstant.Success;
                _augmentedEmployee.MetaData.TotalCount = _bulkUpdateRecordCount;
            }
            else if (_validatedEmployeeList.Count < _distinctEmployeeList.Count)
            {
                _augmentedEmployee.ResultSet.ResultCode = ResultConstant.PartialSuccessCode;
                _augmentedEmployee.ResultSet.ResultDescription = ResultConstant.PartialSuccess;
                _augmentedEmployee.MetaData.TotalCount = _bulkUpdateRecordCount;
            }

        }

        #endregion

    }
}
