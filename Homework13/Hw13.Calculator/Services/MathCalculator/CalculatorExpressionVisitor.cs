using System.Linq.Expressions;

namespace MemoryCachedCalculator.Services.MathCalculator;

public class CalculatorExpressionVisitor : ExpressionVisitor
{
    private readonly Dictionary<Expression, Tuple<Expression, Expression>> _tree = new();

    public Dictionary<Expression, Tuple<Expression, Expression>> GetTree(Expression expression)
    {
        Visit(expression);
        return _tree;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _tree.Add(node, new Tuple<Expression, Expression>(node.Left, node.Right));
        return base.VisitBinary(node);
    }
}
