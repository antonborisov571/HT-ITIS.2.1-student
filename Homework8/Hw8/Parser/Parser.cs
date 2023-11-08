using System.Globalization;
using Hw8.Calculator;

namespace Hw8.Parser;

public class Parser : IParser
{
    public string TryParse(string val1, string val2, string operation, out ParseResult parseResult)
    {
        parseResult = new ParseResult();
        if (!double.TryParse(val1, NumberStyles.Any, CultureInfo.InvariantCulture, out double value1)
            || !double.TryParse(val2, NumberStyles.Any, CultureInfo.InvariantCulture, out double value2))
        {
            return Messages.InvalidNumberMessage;
        }
        parseResult.Value1 = value1;
        parseResult.Value2 = value2;
        parseResult.Operation = operation switch
        {
            "Plus" => Operation.Plus,
            "Minus" => Operation.Minus,
            "Multiply" => Operation.Multiply,
            "Divide" => Operation.Divide,
            _ => Operation.Invalid,
        };

        if (value2 == 0 && parseResult.Operation == Operation.Divide)
        {
            return Messages.DivisionByZeroMessage;
        }

        if (parseResult.Operation == Operation.Invalid)
        {
            return Messages.InvalidOperationMessage;
        }

        return Messages.Ok;
    }


}
