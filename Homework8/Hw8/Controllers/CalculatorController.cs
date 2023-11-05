using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        try
        {
            if (double.TryParse(val1, NumberStyles.Any, CultureInfo.InvariantCulture, out double value1)
            && double.TryParse(val2, NumberStyles.Any, CultureInfo.InvariantCulture, out double value2))
            {
                switch (Enum.Parse(typeof(Operation), operation))
                {
                    case Operation.Plus:
                        return calculator.Plus(value1, value2);
                    case Operation.Minus:
                        return calculator.Minus(value1, value2);
                    case Operation.Multiply:
                        return calculator.Multiply(value1, value2);
                    case Operation.Divide:
                        return calculator.Divide(value1, value2);
                    default:
                        return Content(Messages.InvalidOperationMessage);
                }
            }
            return Content(Messages.InvalidNumberMessage);
        }
        catch (ArgumentException) 
        {
            return Content(Messages.InvalidOperationMessage);
        }
        catch
        {
            return Content(Messages.DivisionByZeroMessage);
        }
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}