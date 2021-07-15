using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nedbank.HR.SAP.BAL.Business;
using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.Serivce.Interfaces;
using Nedbank.HR.SAP.Service.Authentcation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Nedbank.HR.SAP.Serivce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController: ControllerBase, IEmployeesController
    {
        private readonly IEmployeeBusiness _employeeBusiness;
        private readonly IProductBusiness _productBusiness;
        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController(IEmployeeBusiness employeeBusiness, IProductBusiness productBusiness, ILogger<EmployeesController> logger)
        {
            _employeeBusiness = employeeBusiness;
            _productBusiness = productBusiness;
            _logger = logger;
        }

        [HttpGet("{employeenumber}")]
        public async Task<IActionResult> GetAsync([FromRoute]string employeenumber)
        {
            _logger.LogInformation($"Call initiate for Getting employee by id {employeenumber}");

            var augmentedResult = await _employeeBusiness.GetEmployeeByIdAsync(ParseParams());

            if(augmentedResult.ResultSet.ResultCode != null &&
                augmentedResult.ResultSet.ResultCode.Equals(((Int32)HttpStatusCode.NotFound).ToString()))
            {
                _logger.LogInformation($"Employee with ID {employeenumber} not found");
                return NotFound(augmentedResult.ResultSet);
            }
            _logger.LogInformation($"Call completed for Getting employee by id {employeenumber}");
            return Ok(augmentedResult);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {

           
            var result  = await _productBusiness.FetchProductsAsync();
            return Ok(result);
            //_logger.LogInformation($"Call initiate for getting employees with query params {Request.QueryString}");

            //var augmentedResult = await _employeeBusiness.GetEmployeesAsync(ParseParams());

            //if(augmentedResult.ResultSet.ResultCode != null &&
            //    augmentedResult.ResultSet.ResultCode.Equals(((Int32)HttpStatusCode.BadRequest).ToString()))
            //{
            //    _logger.LogInformation($"No query paramters supplied for employee search.");
            //    return BadRequest(augmentedResult.ResultSet);
            //}
            //else if (augmentedResult.ResultSet.ResultCode != null &&
            //    augmentedResult.ResultSet.ResultCode.Equals(((Int32)HttpStatusCode.NotFound).ToString()))
            //{
            //    _logger.LogInformation($"Search of employee(s) with query params {Request.QueryString} not found.");
            //    return NotFound(augmentedResult.ResultSet);
            //}
            //_logger.LogInformation($"Call completed for getting employees with query params {Request.QueryString}.");

            //return Ok(augmentedResult);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] List<Employee> employeelist)
        {
             _logger.LogInformation($"Call initiate for bulk updating {employeelist.Count} number of employee(s).");
            
            if (employeelist == null || employeelist.Count == 0)
            {
                _logger.LogWarning($"No employee(s) received for update.");
                return BadRequest();
            }

            var augmentedResult = await _employeeBusiness.UpdateEmployeesAsync(employeelist);
            _logger.LogInformation($"Bulk updated {augmentedResult.MetaData.TotalCount} employee(s).");
            _logger.LogInformation($"Call completed for bulk updating employee(s).");

            return Ok(augmentedResult);
        }

        [NonAction]
        public IDictionary<string, string> ParseParams()
        {
            var queryParams = new Dictionary<string, string>();
            if (Request.QueryString.HasValue)
            {
                var queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());

                foreach (string query in queryCollection)
                {
                    queryParams.Add(query, queryCollection[query]);
                }
            }
            else if (Request.RouteValues.ContainsKey("employeenumber"))
            {
                queryParams.Add("employeenumber", (string)Request.RouteValues["employeenumber"]);
            }
            return queryParams;
        }

    }
}
