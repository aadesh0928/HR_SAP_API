using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.Serivce.Controllers;
using Nedbank.HR.SAP.Serivce.Interfaces;
using Nedbank.HR.SAP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace Nedbank.HR.SAP.Api.Test
{
    public class EmployeeUnitTestController: IClassFixture<EmployeeFixture>
    {
        #region Members
        private readonly EmployeeFixture _fixture;
        IEmployeesController _controller;
        Mock<IEmployeeBusiness> _employeeBusinessMock;
        Mock<ILogger<EmployeesController>> _loggerMock;
        IDictionary<string, string> _queryParams;
        List<Employee> _bulkEmployeeData;
        #endregion

        #region Ctor
        public EmployeeUnitTestController(EmployeeFixture fixture)
        {
            _fixture = fixture;
            _employeeBusinessMock = _fixture.EmployeeBusiness;
            _loggerMock = _fixture.Logger;
            _queryParams = _fixture.GetQueryParams();
            _controller = new EmployeesController(_employeeBusinessMock.Object, _loggerMock.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.QueryString = new QueryString("?employeename=a");
            _bulkEmployeeData = _fixture.BulkEmployeeData();
        }

        #endregion


        #region Methods

        [Fact]
        public async Task TestGetAllAsync()
        {
            _employeeBusinessMock.Setup(x => x.GetEmployeesAsync(_queryParams))
                    .Returns(Task.FromResult(_fixture.GetAllEmployees()));

            //Act
            var okObjectResult = await _controller.GetAllAsync() as OkObjectResult;
            //Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
            var items = Assert.IsAssignableFrom<IAugmentedResult<List<Employee>>>(okObjectResult.Value).Data;
            Assert.Equal(30000, items.Count);
        }

        [Theory]
        [InlineData("1000")]
        public async Task TestGetByIdAsync(string employeenumber)
        {
            _employeeBusinessMock.Setup(x => x.GetEmployeeByIdAsync(_queryParams))
                .Returns(Task.FromResult(_fixture.GetEmployeeById(employeenumber)));

            var okObjectResult = await _controller.GetAsync(employeenumber) as OkObjectResult;
            Assert.IsType<OkObjectResult>(okObjectResult);

            var item = Assert.IsAssignableFrom<IAugmentedResult<Employee>>(okObjectResult.Value).Data;
            Assert.Equal(employeenumber, item.EmployeeNo);
        }

        [Fact]
        public async Task TestUpdateAsync()
        {
            _employeeBusinessMock.Setup(x => x.UpdateEmployeesAsync(_bulkEmployeeData))
                .Returns(Task.FromResult(_fixture.UpdateEmployee(_bulkEmployeeData)));

            var okObjectResult = await _controller.Put(_bulkEmployeeData) as OkObjectResult;
            Assert.IsType<OkObjectResult>(okObjectResult);

            var item = Assert.IsAssignableFrom<IAugmentedResult<Employee>>(okObjectResult.Value).ResultSet;
            Assert.Equal("R000", item.ResultCode);
        }


        #endregion
        
    }
}
