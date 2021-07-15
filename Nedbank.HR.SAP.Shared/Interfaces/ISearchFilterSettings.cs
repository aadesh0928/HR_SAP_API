using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface ISearchFilterSettings
    {
        Dictionary<string, Dictionary<string, string>> Values { get; set; }
    }

}
