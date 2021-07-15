using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.BAL.Interfaces
{
    public interface IEmployeeBusiness
    {
        Task<IAugmentedResult<List<Employee>>> GetEmployeesAsync(IDictionary<string, string> queryParams);
        Task<IAugmentedResult<Employee>> GetEmployeeByIdAsync(IDictionary<string, string> pathParam);
        Task<IAugmentedResult<Employee>> UpdateEmployeesAsync(List<Employee> employees);

    }
}
