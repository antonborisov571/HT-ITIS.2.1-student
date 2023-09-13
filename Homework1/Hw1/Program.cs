using Hw1;

try
{
    Parser.ParseCalcArguments(
        args, 
        out double arg1, 
        out CalculatorOperation operation, 
        out double arg2);
    Console.WriteLine(Calculator.Calculate(arg1, operation, arg2));
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}