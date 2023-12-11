using MemoryCachedCalculator.ErrorMessages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MemoryCachedCalculator.Services.Validator;

public class ExpressionValidator : IValidator
{
    char[] operations = { '+', '-', '*', '/' };
    char[] brackets = { '(', ')' };
    List<string> errors = new();

    public string? Validate(string? input)
    {
        return ValidateNotEmpty(input) &&
               ValidateBrackets(input!) &&
               ValidateUnknownCharacter(input!) &&
               ValidateOperationPlace(input!) &&
               ValidateNumbersCorrectness(input!)
                ? null
                : errors.First();
    }

    private bool ValidateNotEmpty(string? input)
    {
        if (!string.IsNullOrEmpty(input)) return true;

        errors.Add(MathErrorMessager.EmptyString);
        return false;
    }

    private bool ValidateBrackets(string input)
    {
        var stack = new Stack<char>();
        foreach (var ch in input)
        {
            switch (ch)
            {
                case '(':
                    stack.Push(ch);
                    break;
                case ')' when stack.TryPeek(out var tail) && tail == '(':
                    stack.Pop();
                    break;
                case ')':
                    errors.Add(MathErrorMessager.IncorrectBracketsNumber);
                    return false;
            }
        }

        if (stack.Count <= 0) return true;

        errors.Add(MathErrorMessager.IncorrectBracketsNumber);
        return false;
    }

    private bool ValidateUnknownCharacter(string input)
    {
        foreach (var ch in input.Where(ch =>
                     !char.IsDigit(ch) &&
                     !char.IsWhiteSpace(ch) &&
                     !operations.Contains(ch) &&
                     !brackets.Contains(ch) &&
                     ch != '.'))
        {
            errors.Add(MathErrorMessager.UnknownCharacterMessage(ch));
            return false;
        }

        return true;
    }

    private bool ValidateOperationPlace(string input)
    {
        var stack = new Stack<char>();
        foreach (var ch in input)
        {
            if (operations.Contains(ch))
            {
                switch (stack.TryPeek(out var tail))
                {
                    case true when operations.Contains(tail):
                        errors.Add(MathErrorMessager.TwoOperationInRowMessage(tail.ToString(), ch.ToString()));
                        return false;
                    case true when tail == '(' && ch != '-':
                        errors.Add(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(ch.ToString()));
                        return false;
                    case false when ch != '-':
                        errors.Add(MathErrorMessager.StartingWithOperation);
                        return false;
                    default:
                        stack.Push(ch);
                        break;
                }
            }
            else if (brackets.Contains(ch))
            {
                switch (stack.TryPeek(out var tail))
                {
                    case true when ch == ')' && operations.Contains(tail):
                        errors.Add(MathErrorMessager.OperationBeforeParenthesisMessage(tail.ToString()));
                        return false;
                    default:
                        stack.Push(ch);
                        break;
                }
            }
            else if (!char.IsWhiteSpace(ch))
            {
                stack.Push(ch);
            }
        }

        if (operations.Contains(stack.Pop()))
        {
            errors.Add(MathErrorMessager.EndingWithOperation);
            return false;
        }

        return true;
    }

    private bool ValidateNumbersCorrectness(string input)
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
                    errors.Add(MathErrorMessager.NotNumberMessage(maybeNumber));
                    return false;
                }

                isPreviousDigit = false;
            }
        }

        if (isPreviousDigit)
        {
            var maybeNumber = input[numberStartPos..input.Length];
            if (!double.TryParse(maybeNumber, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                errors.Add(MathErrorMessager.NotNumberMessage(maybeNumber));
                return false;
            }
        }

        return true;
    }
}
