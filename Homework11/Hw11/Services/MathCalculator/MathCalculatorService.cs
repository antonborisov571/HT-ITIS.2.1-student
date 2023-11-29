using Hw11.Dto;
using Hw11.ErrorMessages;
using Hw11.Services.Parsing;
using Hw11.Services.Tokens;
using Hw11.Services.Validator;
using Hw11.Services.Visitor;
using System.Globalization;
using System.Linq.Expressions;


namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    private readonly IValidator _validator;
    private readonly ITokenizer _tokenizer;
    private readonly IParser _parser;

    public MathCalculatorService(IValidator validator, ITokenizer tokenizer, IParser parser)
    {
        _validator = validator;
        _tokenizer = tokenizer;
        _parser = parser;
    }

    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        _validator.Validate(expression);

        var tokens = _tokenizer.Tokenize(expression!);

        var expr = _parser.Parse(tokens);

        var exprVisitor = new CalculatorExpressionVisitor();
        var tree = exprVisitor.GetTree(expr);

        var result = await CalcAsync(expr, tree);
        return double.IsNaN(result)
            ? throw new DivideByZeroException(MathErrorMessager.DivisionByZero)
            : result;
        
    }

    private async Task<double> CalcAsync(Expression current, Dictionary<Expression, Tuple<Expression, Expression?>> tree)
    {
        if (!tree.ContainsKey(current))
        {
            return double.Parse(
                current.ToString(),
                CultureInfo.CurrentCulture);
        }

        if (tree[current].Item2 is null)
        {
            return -1 * await CalcAsync(tree[current].Item1, tree);
        }

        var leftTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalcAsync(tree[current].Item1, tree);
        });
        var rightTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalcAsync(tree[current].Item2!, tree);
        });

        var result = await Task.WhenAll(leftTask, rightTask);

        return current.NodeType switch
        {
            ExpressionType.Add => result[0] + result[1],
            ExpressionType.Subtract => result[0] - result[1],
            ExpressionType.Multiply => result[0] * result[1],
            ExpressionType.Divide when Math.Abs(result[1]) < double.Epsilon => double.NaN,
            _ => result[0] / result[1]
        };
    }
}