using MemoryCachedCalculator.Dto;
using MemoryCachedCalculator.ErrorMessages;
using MemoryCachedCalculator.Services.MathCalculator;
using MemoryCachedCalculator.Services.Parsing;
using MemoryCachedCalculator.Services.Tokens;
using MemoryCachedCalculator.Services.Validator;
using System.Linq.Expressions;

namespace MemoryCachedCalculator.Services.Decorator;

public abstract class CalculatorDecorator : IMathCalculatorService
{
    protected readonly IMathCalculatorService _simpleCalculator;

    public CalculatorDecorator(IMathCalculatorService simpleCalculator)
    {
        _simpleCalculator = simpleCalculator;
    }

    public virtual async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        return await _simpleCalculator.CalculateMathExpressionAsync(expression);
    }
}
