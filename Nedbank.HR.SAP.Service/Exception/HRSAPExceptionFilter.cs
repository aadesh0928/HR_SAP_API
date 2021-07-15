using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nedbank.HR.SAP.BAL.Constants;
using Nedbank.HR.SAP.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nedbank.HR.SAP.Service.Exception
{
    public class HRSAPExceptionFilter : IExceptionFilter
    {
        private IResultSet _resultSet;
        public HRSAPExceptionFilter(IResultSet resultSet)
        {
            _resultSet = resultSet;
        }
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = (int)status;
            context.HttpContext.Response.ContentType = "application/json";

            _resultSet.ResultCode = ResultConstant.FailedCode;
            _resultSet.ResultDescription = ResultConstant.Failed;

            string responseResult = JsonConvert.SerializeObject(_resultSet);
            context.HttpContext.Response.ContentLength = responseResult.Length;
            context.HttpContext.Response.WriteAsync(responseResult);

            context.Exception = null;
            context.ExceptionHandled = true;
        }

    }



}
