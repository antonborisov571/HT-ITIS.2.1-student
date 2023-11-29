using Hw11.Services.Tokens;
using System.Linq.Expressions;

namespace Hw11.Services.Parsing;

public interface IParser
{
    Expression Parse(IEnumerable<Token> tokens);
}
