using MemoryCachedCalculator.Dto;

namespace MemoryCachedCalculator.Services;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}