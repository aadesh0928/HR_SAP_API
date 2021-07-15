using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string Server { get; set; }
        ICredential Credential { get; set; }
    }

    public interface ICredential
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}
