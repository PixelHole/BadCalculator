using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator;

public partial class MainWindow
{
    private float? memory = 0f;
    private float _result;
    private string _equation = "";
    private string Equation
    {
        get => _equation;
        set
        {
            _equation = value;
            UpdateEquationText();
        }
    }
    private float Result
    {
        get => _result;
        set
        {
            _result = value;
            UpdateResultText();
        }
    }

    private int bracketCount = 0;
        
    public MainWindow()
    {
        InitializeComponent();
    }

    private void UpdateEquationText()
    {
        InputField.Text = Equation;
    }
    private void UpdateResultText()
    {
        ResultBox.Text = Result.ToString(CultureInfo.InvariantCulture);
    }
    private void MoveResultToInput()
    {
        if (Result != 0f)
        {
            if (Result > 0) InputField.Text = Result.ToString(CultureInfo.CurrentCulture);
            else InputField.Text = $"({Result.ToString(CultureInfo.CurrentCulture)})";
            ResultBox.Text = String.Empty;
        }
    }
        
    // Numpad Button Trigger functions
    private void _1Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 1;
    }
    private void _2Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 2;
    }
    private void _3Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 3;
    }
    private void _4Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 4;
    }
    private void _5Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 5;
    }
    private void _6Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 6;
    }
    private void _7Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 7;
    }
    private void _8Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 8;
    }
    private void _9Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 9;
    }
    private void _0Btn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += 0;
    }
    private void EulerBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += "e";
    }
    private void PiBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += "pi";
    }
        
        
    // operator Button trigger functions
    private void PowBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MoveResultToInput();
        Equation += "^";
    }
    private void MultiplicationBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MoveResultToInput();
        Equation += "*";
    }
    private void DivideBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MoveResultToInput();
        Equation += "/";
    }
    private void SubtractionBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MoveResultToInput();
        Equation += "-";
    }
    private void AdditionBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MoveResultToInput();    
        Equation += "+";
    }
    private void CosBtn_OnClick(object sender, RoutedEventArgs e)
    {
        bracketCount++;
        Equation += "Cos(";
    }
    private void SinBtn_OnClick(object sender, RoutedEventArgs e)
    {
        bracketCount++;
        Equation += "Sin(";
    }
    private void TanBtn_OnClick(object sender, RoutedEventArgs e)
    {
        bracketCount++;
        Equation += "Tan(";
    }
    private void CotBtn_OnClick(object sender, RoutedEventArgs e)
    {
        bracketCount++;
        Equation += "Cot(";
    }
        
        
    // number operator Button Trigger
    private void ClearBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Result = 0f;
        Equation = "";
        ResultBox.Text = "";
    }
    private void BracketBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (Equation.Length == 0)
        {
            Equation += "(";
            bracketCount++;
            return;
        }
        if (Equation[^1] != '(' && bracketCount > 0)
        {
            Equation += ")";
            bracketCount--;
            return;
        }

        if (Equation[^1] != '(' && bracketCount == 0)
        {
            Equation += "*";
        }

        Equation += "(";
        bracketCount++;
    }
    private void FactorialBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += "!";
    }
    private void SignBtn_OnClick(object sender, RoutedEventArgs e)
    {
        bracketCount++;
        Equation += "(-";
    }
    private void DotBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Equation += ".";
    }
    private void EqualsBtn_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Result = CalculatorCore.SolveEquation(Equation);
        }
        catch (SyntaxErrorException exception)
        {
            ResultBox.Text = exception.Message;
        }
    }
    private void RemoveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (Equation.Length == 0) return;
        
        if (Equation[^1] == ')') bracketCount++;
        if (Equation[^1] == '(') bracketCount--;

        try
        {
            Equation = Equation.Substring(0, Equation.Length - 1);
        }
        catch (SyntaxErrorException exception)
        {
            ResultBox.Text = exception.Message;
        }
    }
        
        
    // Input field text change trigger
    private void InputField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        _equation = InputField.Text;
    }
    
    
    // function button triggers
    private void MinimizeBtn_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    
    // Memory manipulation
    private void AddMemoryBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (memory == null)
        {
            memory = Result;
            return;
        }
        memory += Result;
    }
    private void SubtractMemoryBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (memory == null)
        {
            memory = -Result;
            return;
        }
        memory -= Result;
    }
    private void MemoryStoreBtn_OnClick(object sender, RoutedEventArgs e)
    {
        memory = Result;
    }
    private void MemoryRecallBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (memory == null) return;
        Result = memory.Value;
    }
    private void MemoryClear_OnClick(object sender, RoutedEventArgs e)
    {
        memory = null;
    }
    
    
    // window settings
    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }
}