using MemoryCachedCalculator.Services.Tokens;
using System.Globalization;
using System.Linq.Expressions;

namespace MemoryCachedCalculator.Services.Parsing;

public class Parser : IParser
{
    List<Token> Tokens { get; set; } = null!;
    int Position { get; set; } = 0;

    public Expression Parse(IEnumerable<Token> tokens)
    {
        Tokens = new List<Token>(tokens);
        return ParseFormula();
    }

    Expression ParseFormula()
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
            var formula = ParseFormula();
            GetNext();
            return formula;
        }
        return ParseNumber();
    }

    Expression ParseNumber()
    {
        return Expression.Constant(double.Parse(
            GetNext().Value,
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

}
