using Hw9.ErrorMessages;
using System.Text;

namespace Hw9.Services.Tokens;

public class Tokenizer
{
    string Input { get; }
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

    public Tokenizer(string input)
    {
        Input = string.Join("", input.Split(' '));
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();
        while (!IsEnd())
        {
            var isFoundKey = false;
            foreach (var (key, value) in tokenTypes)
            {
                if (Get(0) == key)
                {
                    tokens.Add(new Token(value, key.ToString()));
                    isFoundKey = true;
                    Position++;
                    break;
                }
            }

            if (!isFoundKey)
            {
                if (char.IsDigit(Get(0)))
                {
                    tokens.Add(TokenizeNumber());
                }
            }
        }

        return tokens;
    }

    private Token TokenizeNumber()
    {
        var sb = new StringBuilder();
        var isFoundDot = false;
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
