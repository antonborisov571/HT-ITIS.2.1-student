using MemoryCachedCalculator.Services.Tokens;
using System.Linq.Expressions;

namespace MemoryCachedCalculator.Services.Parsing;

public interface IParser
{
    Expression Parse(IEnumerable<Token> tokens);
}
