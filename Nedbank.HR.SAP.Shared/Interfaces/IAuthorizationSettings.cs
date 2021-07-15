using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IAuthorizationSettings
    {
        string ClientId { get; set; } 
        string ClientSecret { get; set; }
    }
}
