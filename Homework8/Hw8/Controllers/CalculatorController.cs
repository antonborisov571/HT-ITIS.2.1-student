using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Hw8.Parser;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] IParser parser,
        [FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        var message = parser.TryParse(val1, val2, operation, out ParseResult result);
        if (message == Messages.Ok) 
        {
            return calculator.Calculate(result.Value1, result.Operation, result.Value2);
        }

        return Content(message);
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}