using Hw8.Calculator;

namespace Hw8.Parser;

public interface IParser
{
    string TryParse(string val1, string val2, string operation, out ParseResult parseResult);
}
