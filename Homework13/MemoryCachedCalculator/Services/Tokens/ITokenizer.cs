namespace MemoryCachedCalculator.Services.Tokens;

public interface ITokenizer
{
    List<Token> Tokenize(string expression);
}
