using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nedbank.HR.SAP.Service.Authentcation
{
    public class StaffAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        private readonly IAuthorizationSettings _authorizationSettings;
        public StaffAuthenticationFilter(IAuthorizationSettings authorizationSettings)
        {
            _authorizationSettings = authorizationSettings;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            StringValues clientId, clientSecret;
            context.HttpContext.Request.Headers.TryGetValue("X-IBM-Client-Id", out clientId);
            context.HttpContext.Request.Headers.TryGetValue("X-IBM-Client-Secret", out clientSecret);

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide client Id/ client secret";
                context.Result = new JsonResult("Please Provide client id / client secret")
                {
                    Value = new
                    {
                        Status = "Error",
                        Message = "Please provide authentication headers."
                    },
                };
            }
            else
            {
                if (!this.ValidateParams(clientId, clientSecret))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Result = new UnauthorizedObjectResult($"Incorrect cleint id {clientId} , cleint seceret {clientSecret}");

                }
                
            }
        }

        private bool ValidateParams(string clientId, string clientSeceret)
        {
            bool isValid = true;
            if(!_authorizationSettings.ClientId.Equals(clientId, StringComparison.InvariantCultureIgnoreCase) ||
                !_authorizationSettings.ClientSecret.Equals(clientSeceret, StringComparison.InvariantCultureIgnoreCase))
            {
                isValid =  false;
            }


            return isValid;
        }
    }
}
