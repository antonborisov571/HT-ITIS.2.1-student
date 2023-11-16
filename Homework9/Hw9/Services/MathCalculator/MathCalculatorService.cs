using Hw9.Dto;
using Hw9.ErrorMessages;
using Hw9.Services.Parsing;
using Hw9.Services.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var error = new ExpressionValidator().Validate(expression);
        if (error is not null)
        {
            return new CalculationMathExpressionResultDto(error);
        }

        var tokens = new Tokenizer(expression!).Tokenize();

        var expr = new Parser(tokens).Parse();

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
            return double.Parse(current.ToString(), CultureInfo.InvariantCulture);
        }

        var leftTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalcAsync(tree[current].Item1, tree);
        });
        var rightTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
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