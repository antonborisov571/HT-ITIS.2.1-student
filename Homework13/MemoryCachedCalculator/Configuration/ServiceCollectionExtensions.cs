using MemoryCachedCalculator.Services;
using MemoryCachedCalculator.Services.CachedCalculator;
using MemoryCachedCalculator.Services.MathCalculator;
using MemoryCachedCalculator.Services.Parsing;
using MemoryCachedCalculator.Services.Tokens;
using MemoryCachedCalculator.Services.Validator;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryCachedCalculator.Configuration;

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
                s.GetRequiredService<IMemoryCache>(), 
                s.GetRequiredService<MathCalculatorService>()));
    }
}