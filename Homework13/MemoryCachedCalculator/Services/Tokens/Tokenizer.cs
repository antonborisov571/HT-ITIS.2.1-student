using MemoryCachedCalculator.ErrorMessages;
using System.Text;

namespace MemoryCachedCalculator.Services.Tokens;

public class Tokenizer : ITokenizer
{
    string Input { get; set; } = null!;
    int Position { get; set; } = 0;

    Dictionary<char, TokenType> tokenTypes = new()
    {
        ['+'] = TokenType.Plus,
        ['-'] = TokenType.Minus,
        ['*'] = TokenType.Multiply,
        ['/'] = TokenType.Divide,
        ['('] = TokenType.LBracket,
        [')'] = TokenType.RBracket,
    };

    public List<Token> Tokenize(string expression)
    {
        Input = string.Join("", expression.Split(' '));
        var tokens = new List<Token>();
        while (!IsEnd())
        {
            if (tokenTypes.ContainsKey(Get(0)))
            { 
                tokens.Add(new Token(tokenTypes[Get(0)], Get(0).ToString()));
                Position++;
            }
            else
            {
                tokens.Add(TokenizeNumber()); 
            }
        }

        return tokens;
    }

    private Token TokenizeNumber()
    {
        var sb = new StringBuilder();
        while (char.IsDigit(Get(0)) || Get(0) == '.')
        {
            sb.Append(Get(0));
            Position++;
        }

        return new Token(TokenType.Number, sb.ToString());
    }

    bool IsEnd() => Get(0) == '\0';
    char Get(int position)
    {
        if (position + Position < Input.Length)
            return Input[position + Position];

        return '\0';
    }
}
