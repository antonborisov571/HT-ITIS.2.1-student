namespace Hw8.Calculator;

public interface ICalculator
{
    double Plus(double val1, double val2);
    
    double Minus(double val1, double val2);
    
    double Multiply(double val1, double val2);
    
    double Divide(double val1, double val2);

    double Calculate(double val1, Operation operation, double val2);
}