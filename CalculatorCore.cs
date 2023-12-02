using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Documents;
using BadQueue.BadQueue;
using OfficialBadStackFanClub.BadStack;

namespace Calculator;

public static class CalculatorCore
{
    public static List<MathOperator?> ValidOperators { get; private set; } = new List<MathOperator?>(new []
    {
        new MathOperator("+", 2, true, (a, b) => a + b),
        new MathOperator("-", 2, true, (a, b) => a - b),
        new MathOperator("*", 3, true, (a, b) => a * b),
        new MathOperator("/", 3, true, (a, b) => a / b),
        new MathOperator("^", 4, true, MathF.Pow),
        new MathOperator("!", 5, a => 1),
        new MathOperator("sin", 5, MathF.Sin),
        new MathOperator("cos", 5, MathF.Cos),
        new MathOperator("tan", 5, MathF.Tan),
        new MathOperator("cot", 5, f => 1/MathF.Tan(f)),
    });

    public static List<(string? name, float value)> SpecialNumbers = new List<(string? name, float value)>(new[]
    {
        ("pi", MathF.PI),
        ("e", MathF.E),
    }!);

    public static float SolveEquation(string equation)
    {
        BadStack<float> result = new BadStack<float>();

        BadQueue<(MathOperator? op, float? num)> rpnEq = new BadQueue<(MathOperator? op, float? num)>();
        
        try
        {
            rpnEq = ToRPN(equation);
        }
        catch (Exception e)
        {
            if (e is InvalidOperationException || e is InvalidExpressionException)
            {
                // invalid parenthesis
                return -1;
            }
            else if (e is ArgumentException)
            {
                // unknown operator
                return -2;
            }
        }
        
        int count = rpnEq.Count;
        for (int i = 0; i < count; i++)
        {
            var cell = rpnEq.Dequeue();
            
            if (cell.num != null)
            {
                result.Push(cell.num.Value);
            }
            else if (cell.op != null)
            {
                float? res = null;

                switch (cell.op.InputCount)
                {
                    case 2:
                        float a = 0f, b = 0f;
                        try
                        {
                            a = result.Pop();
                            b = result.Pop();
                        }
                        catch (InvalidOperationException)
                        {
                            return -2;
                        }

                        if (cell.op.LeftToRight)
                        {
                            res = cell.op.DoubleInputResult(b, a);
                            break;
                        }
                        
                        res = cell.op.DoubleInputResult(a, b);
                        break;
                    
                    case 1:
                        res = cell.op.SingleInputResult(result.Pop());
                        break;
                    
                    case 0:
                        break;
                }

                if (res != null)
                {
                    result.Push(res.Value);
                }
            }
        }

        return result.Peek();
    }

    public static BadQueue<(MathOperator? op, float? num)> ToRPN(string equation)
    {
        equation = "(" + equation + ")";
        
        BadQueue<(MathOperator? op, float? num)> output = new BadQueue<(MathOperator? op, float? num)>();
        BadStack<MathOperator> operators = new BadStack<MathOperator>();

        StringBuilder currentWord = new StringBuilder(), currentNumber = new StringBuilder();
        
        for (int i = 0; i < equation.Length; i++)
        {
            // number catching
            try
            {
                int num = int.Parse(equation[i].ToString());
                if (currentWord.Length > 0) throw new ArgumentException();
                currentNumber.Append(equation[i]);
                continue;
            }
            catch (FormatException)
            {
                if (equation[i] == '.')
                {
                    currentNumber.Append(equation[i]);
                    continue;
                }
                else
                {
                    if(currentNumber.Length > 0) output.Enqueue((null, float.Parse(currentNumber.ToString())));
                    currentNumber.Clear();
                }
            }

            // parenthesis logic
            if (equation[i] == '(')
            {
                if (currentWord.Length > 0) throw new ArgumentException();
                operators.Push(new MathOperator("(", 0, () => 0));
                continue;
            }
            else if (equation[i] == ')')
            {
                while (operators.Peek().sign != "(")
                {
                    output.Enqueue((operators.Pop(), null));
                }
                operators.Pop();
                continue;
            }

            
            // operator catching
            currentWord.Append(equation[i]);

            string opString = currentWord.ToString().ToLower();

            MathOperator? foundOp = FindOperatorBySign(opString);

            if (foundOp != null)
            {
                currentWord.Clear();
                
                // negative sign detection
                if (foundOp.sign == "-" && operators.Peek().sign == "(")
                {
                    output.Enqueue((null, 0f));
                }
                
                while (operators.Peek().precedence >= foundOp.precedence)
                {
                    output.Enqueue((operators.Pop() ,null));
                }
                
                operators.Push(foundOp);
                
                continue;
            }

            float? specialCharacter = FindSpecialCharacterByName(opString);

            if (specialCharacter != null)
            {
                currentWord.Clear();
                
                output.Enqueue((null, specialCharacter.Value));
            }
        }

        // push all ops at the end
        foreach (MathOperator op in operators)
        {
            if (op.sign == "(") throw new InvalidExpressionException();
            output.Enqueue((op, null));
        }
        
        return output;
    }

    private static MathOperator? FindOperatorBySign(string sign)
    {
        MathOperator? res = ValidOperators.Find(op => op.sign == sign);

        return res;
    }

    private static float? FindSpecialCharacterByName(string name)
    {
        (string? name, float value) res = SpecialNumbers.Find(tuple => tuple.name == name);

        if (res.name == null) return null;

        return res.value;
    }
}