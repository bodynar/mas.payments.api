namespace MAS.Payments.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Linq.Expressions;

    internal class ExpressionParameterRebinder : ExpressionVisitor
    {
        private Dictionary<ParameterExpression, Expression> ParameterMap { get; }

        public ExpressionParameterRebinder(Dictionary<ParameterExpression, Expression> parameterMap)
        {
            ParameterMap = parameterMap;
        }

        protected override Expression VisitParameter(ParameterExpression parameter)
        {
            return ParameterMap
                .TryGetValue(parameter, out var replacement)
                ? replacement
                : parameter;
        }
    }

    public static class ExpressionExtenstions
    {
        public static Expression<T> Combine<T>(
            this Expression<T> leftExpression, Expression<T> rightExpression,
            Func<Expression, Expression, Expression> mergeFunction)
        {
            if (leftExpression == null)
            {
                throw new ArgumentException(nameof(leftExpression));
            }
            if (rightExpression == null)
            {
                throw new ArgumentException(nameof(rightExpression));
            }
            if (mergeFunction == null)
            {
                throw new ArgumentException(nameof(mergeFunction));
            }

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