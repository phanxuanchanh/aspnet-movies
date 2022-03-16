using System;
using System.Linq.Expressions;

namespace MSSQL.LambdaExpression
{
    internal class ExpressionExtension
    {
        public ExpressionExtension()
        {

        }

        public string ConvertExpressionTypeToString(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "!=";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThanOrEqual: return "<=";
                case ExpressionType.AndAlso: return "and";
                case ExpressionType.OrElse: return "or";
                case ExpressionType.And: return "&";
                case ExpressionType.Or: return "|";
                case ExpressionType.Not: return "not";
                default: return null;
            }
        }
    }
}
