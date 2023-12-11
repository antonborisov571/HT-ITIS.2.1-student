using MemoryCachedCalculator.Dto;
using MemoryCachedCalculator.ErrorMessages;
using MemoryCachedCalculator.Services.CachedCalculator;
using MemoryCachedCalculator.Services.Decorator;
using MemoryCachedCalculator.Services.Parsing;
using MemoryCachedCalculator.Services.Tokens;
using MemoryCachedCalculator.Services.Validator;
using System.Globalization;
using System.Linq.Expressions;


namespace MemoryCachedCalculator.Services.MathCalculator;

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

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var error = _validator.Validate(expression);
        if (error is not null)
        {
            return new CalculationMathExpressionResultDto(error);
        }

        var tokens = _tokenizer.Tokenize(expression!);

        var expr = _parser.Parse(tokens);

        var exprVisitor = new CalculatorExpressionVisitor();
        var tree = exprVisitor.GetTree(expr);

        var result = await CalcAsync(expr, tree);
        return double.IsNaN(result)
            ? new CalculationMathExpressionResultDto(MathErrorMessager.DivisionByZero)
            : new CalculationMathExpressionResultDto(result);
        
    }

    private async Task<double> CalcAsync(Expression current, Dictionary<Expression, Tuple<Expression, Expression>> tree)
    {
        if (!tree.ContainsKey(current))
        {
            return double.Parse(
                current.ToString(),
                CultureInfo.CurrentCulture);
        }

        var leftTask = Task.Run(async () =>
        {
            return await CalcAsync(tree[current].Item1, tree);
        });
        var rightTask = Task.Run(async () =>
        {
            return await CalcAsync(tree[current].Item2, tree);
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