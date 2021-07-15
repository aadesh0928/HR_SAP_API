using Nedbank.HR.SAP.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IAugmentedResult<T> where T: class
    {
        T Data { get; set; }

        IMetaDataResult MetaData { get; }
        IResultSet ResultSet { get; set; }
        List<IValidationResult<T>> ValidationResult { get; set; }
    }
}
