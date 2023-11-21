using Homework10.Migrations;
using Hw10.DbModels;
using Hw10.Dto;
using Hw10.ErrorMessages;
using Hw10.Services.Decorator;
using Hw10.Services.MathCalculator;
using Hw10.Services.Parsing;
using Hw10.Services.Tokens;
using Hw10.Services.Validator;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : CalculatorDecorator
{
	private readonly ApplicationContext _dbContext;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator) 
		: base(simpleCalculator)
	{
		_dbContext = dbContext;
	}

	public override async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var foundSolvingExpression = _dbContext.SolvingExpressions.FirstOrDefault(x => x.Expression == expression);
		if (foundSolvingExpression is not null && foundSolvingExpression.Result != 0)
			return new CalculationMathExpressionResultDto(foundSolvingExpression.Result);
		
		var result = await base.CalculateMathExpressionAsync(expression);
		if (!result.IsSuccess)
			return result;
		var count = _dbContext.SolvingExpressions.Count();
		_dbContext.Add(new SolvingExpression()
		{
			SolvingExpressionId = count + 1,
			Expression = expression!,
			Result = result.Result
		});
		await _dbContext.SaveChangesAsync();
		return result;
    }
}