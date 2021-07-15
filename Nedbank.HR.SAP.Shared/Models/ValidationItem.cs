using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{
    public class ValidationItem
    {
        public ValidationItem(string name, string message, object value)
        {
            Name = name;
            Message = message;
            Value = value;
        }
        public string Name { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
    }
}
