using Microsoft.AspNetCore.Mvc;
using Nedbank.HR.SAP.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.Serivce.Interfaces
{
    public interface IEmployeesController
    {
        Task<IActionResult> GetAsync([FromRoute] string employeenumber);
        Task<IActionResult> GetAllAsync();
        Task<IActionResult> Put([FromBody] List<Employee> employees);

        ControllerContext ControllerContext { get; set; }
    }
}
