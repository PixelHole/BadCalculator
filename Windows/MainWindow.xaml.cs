using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string result = "";

            try
            {
                result = CalculatorCore.SolveEquation(Input.Text).ToString();
            }
            catch (SyntaxErrorException exception)
            {
                Output.Text = exception.Message;
                return;
            }

            Output.Text = result;
        }
    }
}
