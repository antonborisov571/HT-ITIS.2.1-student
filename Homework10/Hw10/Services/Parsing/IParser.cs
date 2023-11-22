using Hw10.Services.Tokens;
using System.Linq.Expressions;

namespace Hw10.Services.Parsing;

public interface IParser
{
    Expression Parse(IEnumerable<Token> tokens);
}
