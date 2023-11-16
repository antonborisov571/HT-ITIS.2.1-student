using Hw9.Services.Tokens;
using System.Globalization;
using System.Linq.Expressions;

namespace Hw9.Services.Parsing;

public class Parser
{
    List<Token> Tokens { get; }
    int Position { get; set; } = 0;

    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }

    public Expression Parse()
    {
        var leftOperand = ParseAddOrSubstract();
        return leftOperand;
    }

    Expression ParseAddOrSubstract()
    {
        var result = ParseMultiplyOrDivide();
        while (!IsEnd())
        {
            if (CheckNext(TokenType.Plus))
            {
                result = Expression.Add(result, ParseMultiplyOrDivide());
                continue;
            }
            if (CheckNext(TokenType.Minus))
            {
                result = Expression.Subtract(result, ParseMultiplyOrDivide());
                continue;
            }
            break;
        }
        return result;
    }

    Expression ParseMultiplyOrDivide()
    {
        var result = ParseNegate();
        while (!IsEnd())
        {
            if (CheckNext(TokenType.Multiply))
            {
                result = Expression.Multiply(result, ParseNegate());
                continue;
            }
            if (CheckNext(TokenType.Divide))
            {
                result = Expression.Divide(result, ParseNegate());
                continue;
            }
            break;
        }
        return result;
    }

    Expression ParseNegate()
    {
        if (CheckNext(TokenType.Minus))
        {
            return Expression.Negate(ParseParenthesis());
        }
        return ParseParenthesis();
    }

    Expression ParseParenthesis()
    {
        if (CheckNext(TokenType.LBracket))
        {
            var formula = Parse();
            Expectation(TokenType.RBracket);
            return formula;
        }
        return ParseNumber();
    }

    Expression ParseNumber()
    {
        return Expression.Constant(double.Parse(
            Expectation(TokenType.Number).Value,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture), 
            typeof(double)); 
    }

    Token GetNext() => Tokens[Position++];

    bool Check(params TokenType[] tokens) => tokens.Contains(Tokens[Position].Type);

    bool IsEnd() => Position >= Tokens.Count;
        

    bool CheckNext(params TokenType[] tokens)
    {
        if (!Check(tokens))
            return false;
        Position++;
        return true;
    }

    Token Expectation(params TokenType[] tokens) =>
        Check(tokens)
        ? GetNext()
        : throw new ArgumentException();
}
