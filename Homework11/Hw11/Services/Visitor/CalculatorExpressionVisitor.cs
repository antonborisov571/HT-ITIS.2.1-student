using System.Linq.Expressions;
using System.Reflection;

namespace Hw11.Services.Visitor;

public class CalculatorExpressionVisitor 
{
    private readonly Dictionary<Expression, Tuple<Expression, Expression?>> _tree = new();

    public Dictionary<Expression, Tuple<Expression, Expression?>> GetTree(Expression expression)
    {

        Visit((dynamic)expression);
        return _tree;
    }

    public void Visit(BinaryExpression node)
    {
        _tree.Add(node, new Tuple<Expression, Expression?>(node.Left, node.Right));
        Visit((dynamic)node.Left);
        Visit((dynamic)node.Right);
    }

    public void Visit(UnaryExpression node)
    {
        _tree.Add(node, new Tuple<Expression, Expression?>(node.Operand, null));
        Visit((dynamic)node.Operand);
    }

    public void Visit(ConstantExpression node) { }
}
