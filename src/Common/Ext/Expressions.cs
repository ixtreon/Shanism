using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Shanism.Common
{
    public static class Expressions
    {

        public static Func<T, object> CreateGetter<T>(this PropertyInfo mi)
            => CreateGetter<T, object>(mi);

        public static Func<TFrom, TTo> CreateGetter<TFrom, TTo>(this PropertyInfo pi)
        {
            var param = Expression.Parameter(typeof(TFrom));

            Expression body = Expression.MakeMemberAccess(param, pi);
            if (pi.PropertyType != typeof(TTo))
                body = Expression.Convert(body, typeof(TTo));

            var lambda = Expression.Lambda<Func<TFrom, TTo>>(body, param);
            return lambda.Compile();
        }

        public static Action<TFrom, TTo> CreateSetter<TFrom, TTo>(this PropertyInfo pi)
        {
            var pObj = Expression.Parameter(typeof(TFrom));
            var pVal = Expression.Parameter(typeof(TTo));

            var pLeft = Expression.MakeMemberAccess(pObj, pi);
            var pRight = (pi.PropertyType == typeof(TTo))
                ? (Expression)pVal
                : Expression.Convert(pVal, typeof(TTo));

            var body = Expression.Assign(pLeft, pRight);

            var lambda = Expression.Lambda<Action<TFrom, TTo>>(body, pObj, pVal);
            return lambda.Compile();
        }


        class ExpressionReplacementVisitor : ExpressionVisitor
        {
            readonly Expression oldExpression, newExpression;

            public ExpressionReplacementVisitor(Expression oldExpression, Expression newExpression)
            {
                if (oldExpression.Type != newExpression.Type)
                    throw new ArgumentException($"The replaced parameter type `{oldExpression.Type.FullName}` does not match the expression type `{newExpression.Type.FullName}`.");

                this.oldExpression = oldExpression;
                this.newExpression = newExpression;
            }

            public override Expression Visit(Expression node)
            {
                if (node == oldExpression)
                    return base.Visit(newExpression);

                return base.Visit(node);
            }
        }

        public static Expression Replace<T>(this T source, Expression oldExpression, Expression newExpression)
            where T : Expression
        {
            var visitor = new ExpressionReplacementVisitor(oldExpression, newExpression);
            return visitor.Visit(source);
        }
    }
}
