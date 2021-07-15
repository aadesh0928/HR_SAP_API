using Nedbank.HR.SAP.BAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nedbank.HR.SAP.BAL.Interfaces
{
    public interface IExpressionFilter<T> where T: class
    {
        List<IFilterStatement> Statements { get; set; }

        Expression<Func<T, bool>> BuildExpression();

        Expression<Func<T, bool>> BuildExpressionForNestedType(string propNames, object value);
    }


    public interface IFilterStatement
    {
        string PropertyName { get; set; }
        public FilterOperation FilterOperation { get; set; }
        object Value { get; set; }
    }
}
