using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{
    public class ResultSet : IResultSet
    {
        public string ResultCode { get; set; }
        public string ResultDescription { get; set; }
    }
}
