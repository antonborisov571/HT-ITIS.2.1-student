using Hw9.Services.MathCalculator;
using Hw9.Services.Parsing;
using Hw9.Services.Tokens;
using Hw9.Services.Validator;

namespace Hw9.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IMathCalculatorService, MathCalculatorService>()
            .AddTransient<IValidator, ExpressionValidator>()
            .AddTransient<ITokenizer, Tokenizer>()
            .AddTransient<IParser, Parser>();
    }
}