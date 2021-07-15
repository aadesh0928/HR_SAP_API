using MongoDB.Driver;
using Nedbank.HR.SAP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<HR_SAP_Employee>> FetchEmployeesAsync(Expression<Func<HR_SAP_Employee, bool>> predicate);
        Task<long> BulkUpdateEmployeeAsync(List<HR_SAP_Employee> employees);
    }
}
