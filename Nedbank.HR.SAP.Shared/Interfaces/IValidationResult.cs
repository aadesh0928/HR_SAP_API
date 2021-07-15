using Nedbank.HR.SAP.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Interfaces
{
    public interface IValidationResult<T> where T:class
    {
        T DataItem { get; set; }
        List<ValidationItem> Validations { get; set; }

    }
}
