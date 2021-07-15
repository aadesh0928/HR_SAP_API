using MongoDB.Driver;
using Nedbank.HR.SAP.DAL.Interfaces;
using Nedbank.HR.SAP.DAL.Models;
using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.DAL.Repository
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly IMongoCollection<HR_SAP_Employee> _employees;
        private List<WriteModel<HR_SAP_Employee>> _employeeBulkInsertRequests;
        public EmployeeRepository(IMongoConnector<HR_SAP_Employee> connector)
        {
            _employees = connector.Collection;

            if (_employeeBulkInsertRequests == null)
            {
                _employeeBulkInsertRequests = new List<WriteModel<HR_SAP_Employee>>();
            }
        }
        public async Task<List<HR_SAP_Employee>> FetchEmployeesAsync(Expression<Func<HR_SAP_Employee, bool>> predicate)
        {
            try
            {
                return await _employees.Find<HR_SAP_Employee>(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<long> BulkUpdateEmployeeAsync(List<HR_SAP_Employee> employees)
        {
            try
            {
                // Delete all employees
                 await BulkDeleteEmployeeAsync();

                // Bulk insert employees
                _employeeBulkInsertRequests = employees.Select(emp => new InsertOneModel<HR_SAP_Employee>(emp)).ToList<WriteModel<HR_SAP_Employee>>();
                var result = await _employees.BulkWriteAsync(_employeeBulkInsertRequests, new BulkWriteOptions
                {
                    IsOrdered = false
                });

                return result.InsertedCount;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task BulkDeleteEmployeeAsync()
        {
            try
            {
                await _employees.DeleteManyAsync<HR_SAP_Employee>(item=> true);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
