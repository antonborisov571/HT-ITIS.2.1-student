using Hw11.Services.Parsing;
using Hw11.Services.Tokens;
using Hw11.Services.Validator;
using Hw11.Services.MathCalculator;
using Hw11.Exceptions;

namespace Hw11.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IMathCalculatorService, MathCalculatorService>()
            .AddTransient<IExceptionHandler, ExceptionHandler>()
            .AddTransient<IValidator, ExpressionValidator>()
            .AddTransient<ITokenizer, Tokenizer>()
            .AddTransient<IParser, Parser>();
    }
}