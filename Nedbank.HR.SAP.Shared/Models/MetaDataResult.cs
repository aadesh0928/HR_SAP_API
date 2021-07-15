using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{
    public class MetaDataResult: IMetaDataResult
    {
        public long? TotalCount { get; set; }
    }
}
