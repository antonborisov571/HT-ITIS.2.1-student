﻿namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args, 
        out double val1, 
        out CalculatorOperation operation, 
        out double val2)
    {
        if (!IsArgLengthSupported(args))
        {
            throw new ArgumentException();
        }

        if (!double.TryParse(args[0], out val1))
        {
            throw new ArgumentException();
        }

        operation = ParseOperation(args[1]);

        if (!double.TryParse(args[2], out val2))
        {
            throw new ArgumentException();
        }
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg)
    {
        switch (arg)
        {
            case "+":
                return CalculatorOperation.Plus;
            case "-":
                return CalculatorOperation.Minus;
            default:
                throw new InvalidOperationException();
        }
    }
}