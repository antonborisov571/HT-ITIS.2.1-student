namespace Hw11.Services.Tokens;

public interface ITokenizer
{
    List<Token> Tokenize(string expression);
}
