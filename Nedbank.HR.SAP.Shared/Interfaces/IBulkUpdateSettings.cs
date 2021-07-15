using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IBulkUpdateSettings
    {
        int ValidationRecordsCapping { get; set; }
    }
}
