using Nedbank.HR.SAP.BAL.Enums;
using Nedbank.HR.SAP.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
namespace Nedbank.HR.SAP.BAL.Filters
{
    public class ExpressionFilter<T> : IExpressionFilter<T> where T : class
    {
        public ExpressionFilter()
        {
            Statements = new List<IFilterStatement>();
        }
        public List<IFilterStatement> Statements { get; set; }
        public Expression<Func<T, bool>> BuildExpression()
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "item");
                Expression OrElseExpression = Expression.Constant(false);
                Expression AndAlsoExpression = Expression.Constant(true);
                Expression finalExpression = Expression.Constant(true);
                Statements.ForEach(filterSatement =>
                {
                    var propertNames = filterSatement.PropertyName;
                    var constant = Expression.Constant(filterSatement.Value); // 'engnr,b.ankystr'
                    if (propertNames.IndexOf(",") != -1) // Multi filter properties
                    {
                        var propNames = propertNames.Split(',');
                        foreach (var prop in propNames)
                        {
                            var memberExpr = Expression.Property(parameter, prop);
                            OrElseExpression = Expression.OrElse(OrElseExpression, CreateExpressionBody(filterSatement.FilterOperation, memberExpr, filterSatement.Value));
                        }
                    }
                    else
                    {
                        var memberExpr = Expression.Property(parameter, filterSatement.PropertyName);
                        AndAlsoExpression = Expression.AndAlso(AndAlsoExpression, CreateExpressionBody(filterSatement.FilterOperation, memberExpr, filterSatement.Value));
                    }
                });

                if (OrElseExpression.NodeType == ExpressionType.Constant)
                {
                    OrElseExpression = Expression.Constant(true);
                }
                finalExpression = Expression.AndAlso(AndAlsoExpression, OrElseExpression);
                return Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Expression<Func<T, bool>> BuildExpressionForNestedType(string propNamesStr, object value)
        {
            try
            {

                //var parameter = Expression.Parameter(typeof(T), "e");
                //var members = propNamesStr.Split('.');
                //var body = NewObject(typeof(T), parameter, members.Select(m => m.Split('.')));
                //return Expression.Lambda<Func<T, bool>>(body, parameter);

                var bindings = new List<MemberBinding>();
                ParameterExpression p = Expression.Parameter(typeof(T), "item");
                Expression finalExpression = Expression.Constant(true);
                var propNames = propNamesStr.Split('.');

                MemberExpression augExpression = null;//

                foreach (var prop in propNames)
                {
                    var member = Expression.PropertyOrField(p, prop);
                    bindings.Add(Expression.Bind(member.Member, member));
                }

                var er = Expression.MemberInit(Expression.New(typeof(T)), bindings);

                finalExpression = CreateExpressionBody(FilterOperation.EqualTo, augExpression, value);

                return Expression.Lambda<Func<T, bool>>(finalExpression, p);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        //static Expression NewObject(Type targetType, Expression source, IEnumerable<string[]> memberPaths, int depth = 0)
        //{
        //    var bindings = new List<MemberBinding>();
        //    var target = Expression.Constant(null, targetType);
        //    foreach (var memberGroup in memberPaths.GroupBy(path => path[depth]))
        //    {
        //        var memberName = memberGroup.Key;
        //        var targetMember = Expression.PropertyOrField(target, memberName);
        //        var sourceMember = Expression.PropertyOrField(source, memberName);
        //        var childMembers = memberGroup.Where(path => depth + 1 < path.Length);
        //        var targetValue = !childMembers.Any() ? sourceMember :
        //            NewObject(targetMember.Type, sourceMember, childMembers, depth + 1);
        //        bindings.Add(Expression.Bind(targetMember.Member, targetValue));
        //    }
        //    return Expression.MemberInit(Expression.New(targetType), bindings);
        //}

        private Expression CreateExpressionBody(FilterOperation filterOperation, MemberExpression member, object value)
        {
            Expression body = null;
            switch (filterOperation)
            {
                case FilterOperation.EqualTo:
                    body = Expression.Equal(member, Expression.Constant(value));
                    break;
                case FilterOperation.GreaterThanOrEqualTo:
                    body = Expression.GreaterThanOrEqual(member, Expression.Constant(value));
                    break;
                case FilterOperation.Contains:
                    MethodInfo methodContains = member.Type.GetMethod("Contains", new Type[] { typeof(string) });
                    MethodInfo methodLower = member.Type.GetMethod("ToLower", System.Type.EmptyTypes);
                    body = Expression.Call(Expression.Call(member, methodLower), methodContains, Expression.Constant(value.ToString().Trim().ToLower()));
                    break;
            }

            return body;
        }
    }

    public class FilterStatement : IFilterStatement
    {
        public string PropertyName { get; set; }
        public FilterOperation FilterOperation { get; set; }
        public object Value { get; set; }
    }
}
