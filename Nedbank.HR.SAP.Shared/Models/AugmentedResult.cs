using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.Shared.Models
{
    public class AugmentedResult<T>: IAugmentedResult<T> where T: class
    {
        public AugmentedResult(IResultSet resultSet, IMetaDataResult metaDataResult)
        {
            MetaData = metaDataResult;
            ResultSet = resultSet;
            ValidationResult = new List<IValidationResult<T>>();
        }
        public T Data { get; set; }
        public IResultSet ResultSet { get; set; }
        public IMetaDataResult MetaData { get; set; }
        public List<IValidationResult<T>> ValidationResult { get; set; }

    }
}
