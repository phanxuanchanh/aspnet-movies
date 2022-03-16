
namespace MSSQL.LambdaExpression
{
    internal class ExpressionTree
    {
        public ExpressionData Data { get; set; }
        public ExpressionTree Left { get; set; }
        public ExpressionTree Right { get; set; }
    }
}
