using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IMetaDataResult
    {
        long? TotalCount { get; set; }
    }
}
