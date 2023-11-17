using Hw9.Services.Tokens;
using System.Linq.Expressions;

namespace Hw9.Services.Parsing;

public interface IParser
{
    Expression Parse(IEnumerable<Token> tokens);
}
