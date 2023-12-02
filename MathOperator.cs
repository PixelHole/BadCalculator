namespace Calculator;

public class MathOperator
{
    public string sign { get; private set; }
    public int precedence { get; private set; }
    
    public int InputCount { get; private set; }
    public bool LeftToRight { get; private set; }

    public delegate float DoubleInputAction(float a, float b);
    public delegate float SingleInputAction(float a);
    public delegate float NoInputAction();

    public DoubleInputAction DoubleInputResult;
    public SingleInputAction SingleInputResult;
    public NoInputAction NoInputResult;

    public MathOperator(string sign, int precedence, bool leftToRight, DoubleInputAction doubleInputAction)
    {
        InputCount = 2;
        this.LeftToRight = leftToRight;
        this.sign = sign;
        this.precedence = precedence;
        DoubleInputResult = doubleInputAction;
    }
    
    public MathOperator(string sign, int precedence, SingleInputAction singleInputAction)
    {
        InputCount = 1;
        this.sign = sign;
        this.precedence = precedence;
        SingleInputResult = singleInputAction;
    }
    
    public MathOperator(string sign, int precedence, NoInputAction noInputAction)
    {
        InputCount = 0;
        this.sign = sign;
        this.precedence = precedence;
        NoInputResult = noInputAction;
    }
}