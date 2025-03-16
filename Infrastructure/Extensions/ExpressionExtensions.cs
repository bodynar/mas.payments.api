namespace MAS.Payments.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class ExpressionParameterRebinder(
        Dictionary<ParameterExpression, Expression> parameterMap
    ) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return parameterMap
                .TryGetValue(node, out var replacement)
                ? replacement
                : node;
        }
    }

    public static class ExpressionExtensions
    {
        public static Expression<T> Combine<T>(
            this Expression<T> leftExpression, Expression<T> rightExpression,
            Func<Expression, Expression, Expression> mergeFunction)
        {
            ArgumentNullException.ThrowIfNull(leftExpression);
            ArgumentNullException.ThrowIfNull(rightExpression);
            ArgumentNullException.ThrowIfNull(mergeFunction);

            if (leftExpression.Parameters.Count != rightExpression.Parameters.Count)
            {
                throw new ArgumentException("Parameters count in expressions not equal");
            }

            var parametersMap = rightExpression.Parameters
                .Select((rightParamener, index) => new
                {
                    rightParamener,
                    leftParameter = leftExpression.Parameters[index]
                })
                .ToDictionary(relation => relation.rightParamener, relation => (Expression)relation.leftParameter);

            foreach (var parameterItem in parametersMap)
            {
                if (parameterItem.Key.Type != parameterItem.Value.Type)
                {
                    throw new ArgumentException("Parameters types not equal");
                }
            }

            var rightExpressionBody = new ExpressionParameterRebinder(parametersMap).Visit(rightExpression.Body);

            return Expression.Lambda<T>(mergeFunction(leftExpression.Body, rightExpressionBody), leftExpression.Parameters);
        }
    }
}