using System.Linq.Expressions;

namespace Estudos.Exame.Capitulo2.GenerateCodeAtRunTime.ExpressionTree
{
    public class MultiplyToAddExpressionVisitor : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Multiply)
            {
                var lef = Visit(node.Left);
                var right = Visit(node.Right);
                return Expression.Add(lef!, right!);
            }
            return base.VisitBinary(node);
        }
    }
}