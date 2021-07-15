using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IResultSet
    {
        string ResultCode { get; set; }
        string ResultDescription { get; set; }

    }
}
