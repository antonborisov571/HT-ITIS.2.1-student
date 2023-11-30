using Hw11.ErrorMessages;
using Hw11.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Hw11.Services.Validator;

public class ExpressionValidator : IValidator
{
    char[] operations = { '+', '-', '*', '/' };
    char[] brackets = { '(', ')' };

    public void Validate(string? input)
    {
        ValidateNotEmpty(input);
        ValidateBrackets(input!);
        ValidateUnknownCharacter(input!);
        ValidateOperationPlace(input!);
        ValidateNumbersCorrectness(input!);
    }

    private void ValidateNotEmpty(string? input)
    {
        if (!string.IsNullOrEmpty(input)) return;

        throw new InvalidSyntaxException(MathErrorMessager.EmptyString);
    }

    private void ValidateBrackets(string input)
    {
        var stack = new Stack<char>();
        foreach (var symbol in input)
        {
            switch (symbol)
            {
                case '(':
                    stack.Push(symbol);
                    break;
                case ')' when stack.TryPeek(out var tail) && tail == '(':
                    stack.Pop();
                    break;
                case ')':
                    throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
                    
            }
        }

        if (stack.Count <= 0) return;

        throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
    }

    private void ValidateUnknownCharacter(string input)
    {
        foreach (var symbol in input.Where(ch =>
                     !char.IsDigit(ch) &&
                     !char.IsWhiteSpace(ch) &&
                     !operations.Contains(ch) &&
                     !brackets.Contains(ch) &&
                     ch != '.'))
        {
            throw new InvalidSymbolException(MathErrorMessager.UnknownCharacterMessage(symbol));
        }
    }

    private void ValidateOperationPlace(string input)
    {
        var stack = new Stack<char>();
        foreach (var symbol in input)
        {
            if (operations.Contains(symbol))
            {
                switch (stack.TryPeek(out var tail))
                {
                    case true when operations.Contains(tail):
                        throw new InvalidSyntaxException(MathErrorMessager.TwoOperationInRowMessage(tail.ToString(), symbol.ToString()));
                    case true when tail == '(' && symbol != '-':
                        throw new InvalidSyntaxException(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(symbol.ToString()));
                    case false when symbol != '-':
                        throw new InvalidSyntaxException(MathErrorMessager.StartingWithOperation);
                    default:
                        stack.Push(symbol);
                        break;
                }
            }
            else if (brackets.Contains(symbol))
            {
                switch (stack.TryPeek(out var tail))
                {
                    case true when symbol == ')' && operations.Contains(tail):
                        throw new InvalidSyntaxException(MathErrorMessager.OperationBeforeParenthesisMessage(tail.ToString()));
                    default:
                        stack.Push(symbol);
                        break;
                }
            }
            else if (!char.IsWhiteSpace(symbol))
            {
                stack.Push(symbol);
            }
        }

        if (operations.Contains(stack.Pop()))
        {
            throw new InvalidSyntaxException(MathErrorMessager.EndingWithOperation);
        }
    }

    private void ValidateNumbersCorrectness(string input)
    {
        var numberStartPos = 0;
        var isPreviousDigit = false;

        for (var i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]) && !isPreviousDigit) continue;

            if (char.IsDigit(input[i]) && !isPreviousDigit)
            {
                numberStartPos = i;
                isPreviousDigit = true;
            }
            else if (char.IsDigit(input[i]) && isPreviousDigit || input[i] == '.')
            {
                isPreviousDigit = true;
            }
            else if (!char.IsDigit(input[i]) && isPreviousDigit)
            {
                var maybeNumber = input[numberStartPos..i];
                if (!double.TryParse(maybeNumber, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
                {
                    throw new InvalidNumberException(MathErrorMessager.NotNumberMessage(maybeNumber));
                }

                isPreviousDigit = false;
            }
        }

        if (isPreviousDigit)
        {
            var maybeNumber = input[numberStartPos..input.Length];
            if (!double.TryParse(maybeNumber, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                throw new InvalidNumberException(MathErrorMessager.NotNumberMessage(maybeNumber));
            }
        }
    }
}
