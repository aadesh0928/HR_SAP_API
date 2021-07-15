using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{
    public class DatabaseSettings: IDatabaseSettings
    {
        public DatabaseSettings()
        {
            Credential = new Credential();
        }
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Server { get; set; }
        public ICredential Credential { get; set; }
    }

    public class  Credential: ICredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
