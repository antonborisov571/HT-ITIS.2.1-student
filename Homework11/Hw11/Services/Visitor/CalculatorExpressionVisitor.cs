using System.Linq.Expressions;
using System.Reflection;

namespace Hw11.Services.Visitor;

public class CalculatorExpressionVisitor : ExpressionVisitor
{
    private readonly Dictionary<Expression, Tuple<Expression, Expression?>> _tree = new();

    public Dictionary<Expression, Tuple<Expression, Expression?>> GetTree(Expression expression)
    {

        Visit((dynamic)expression);
        return _tree;
    }

    public Expression? Visit(BinaryExpression node)
    {
        _tree.Add(node, new Tuple<Expression, Expression?>(node.Left, node.Right));
        Visit((dynamic)node.Left);
        Visit((dynamic)node.Right);
        return node;
    }

    public Expression? Visit(UnaryExpression node)
    {
        _tree.Add(node, new Tuple<Expression, Expression?>(node.Operand, null));
        Visit((dynamic)node.Operand);
        return node;
    }

    public Expression? Visit(ConstantExpression node) => node;
}
