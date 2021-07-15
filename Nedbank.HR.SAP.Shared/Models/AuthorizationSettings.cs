using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{
    public class AuthorizationSettings : IAuthorizationSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
