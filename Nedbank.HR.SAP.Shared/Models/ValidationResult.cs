using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{

    public class ValidationResult<T>: IValidationResult<T> where T:class
    {
        public List<ValidationItem> Validations { get; set; }
        public T DataItem { get; set; }
    }
}
