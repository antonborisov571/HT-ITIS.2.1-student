using Hw10.DbModels;
using Hw10.Services;
using Hw10.Services.CachedCalculator;
using Hw10.Services.MathCalculator;
using Hw10.Services.Parsing;
using Hw10.Services.Tokens;
using Hw10.Services.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace Hw10.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<MathCalculatorService>()
            .AddTransient<IValidator, ExpressionValidator>()
            .AddTransient<ITokenizer, Tokenizer>()
            .AddTransient<IParser, Parser>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<ApplicationContext>(), 
                s.GetRequiredService<MathCalculatorService>()));
    }
}