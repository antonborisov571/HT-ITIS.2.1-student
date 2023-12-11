using MemoryCachedCalculator.Dto;
using MemoryCachedCalculator.ErrorMessages;
using MemoryCachedCalculator.Services.Decorator;
using MemoryCachedCalculator.Services.MathCalculator;
using MemoryCachedCalculator.Services.Parsing;
using MemoryCachedCalculator.Services.Tokens;
using MemoryCachedCalculator.Services.Validator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Linq.Expressions;

namespace MemoryCachedCalculator.Services.CachedCalculator;

public class MathCachedCalculatorService : CalculatorDecorator
{
    private readonly IMemoryCache _memoryCache;

    public MathCachedCalculatorService(IMemoryCache memoryCache, IMathCalculatorService simpleCalculator) 
		: base(simpleCalculator)
	{
		_memoryCache = memoryCache;
	}

	public override async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		if (_memoryCache.TryGetValue(expression, out CalculationMathExpressionResultDto foundSolvingExpression))
			return new CalculationMathExpressionResultDto(foundSolvingExpression.Result);
		
		var result = await base.CalculateMathExpressionAsync(expression);
		if (!result.IsSuccess)
			return result;

		_memoryCache.Set(expression, result);

		return result;
    }
}