using Hw10.Dto;
using Hw10.ErrorMessages;
using Hw10.Services.MathCalculator;
using Hw10.Services.Parsing;
using Hw10.Services.Tokens;
using Hw10.Services.Validator;
using System.Linq.Expressions;

namespace Hw10.Services.Decorator;

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
