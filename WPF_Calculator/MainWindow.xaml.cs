using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calc;
        public MainWindow()
        {
            InitializeComponent();
            calc = new Calculator();
            calc.DidUpdateValue += Calc_DidUpdateValue;
            calc.InputError += calc_Error;
            calc.CalculationError += calc_Error;
        }

        private void calc_Error(object sender, string e)
        {
            MessageBox.Show(e, "Calculator Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Calc_DidUpdateValue(Calculator sender, double value, int precision)
        {
            if (precision > 0)
                output.Text = String.Format("{0:F" + precision + "}", value);
            else output.Text = $"{value}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            string text = button.Content.ToString();
            object tag = button.Tag;

            int digit;
            if (int.TryParse(text, out digit))
            {
                calc.AddDigit(digit);
            }
            else
            {
                switch (tag)
                {
                    case "decimal":
                        calc.AddDecimalPoint();
                        break;
                    case "evaluate":
                        calc.Compute();
                        break;
                    case "addition":
                        calc.AddOperation(Operation.Add);
                        break;
                    case "subtraction":
                        calc.AddOperation(Operation.Sub);
                        break;
                    case "multiplication":
                        calc.AddOperation(Operation.Mul);
                        break;
                    case "division":
                        calc.AddOperation(Operation.Div);
                        break;
                    case "sin":
                        calc.AddOperation(Operation.Sin);
                        calc.Compute();
                        break;
                    case "sqrt":
                        calc.AddOperation(Operation.Sqrt);
                        calc.Compute();
                        break;
                    case "pow":
                        calc.AddOperation(Operation.Pow);
                        break;
                    case "cos":
                        calc.AddOperation(Operation.Cos);
                        calc.Compute();
                        break;
                    case "ln":
                        calc.AddOperation(Operation.Ln);
                        calc.Compute();
                        break;
                    case "log10":
                        calc.AddOperation(Operation.Log10);
                        calc.Compute();
                        break;
                    case "clear_symbol":
                        calc.ClearSymbol();
                        break;
                    case "reset":
                        calc.Reset();
                        break;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D1:
                case Key.NumPad1:
                    calc.AddDigit(1);
                    break;
                case Key.D2:
                case Key.NumPad2:
                    calc.AddDigit(2);
                    break;
                case Key.D3:
                case Key.NumPad3:
                    calc.AddDigit(3);
                    break;
                case Key.D4:
                case Key.NumPad4:
                    calc.AddDigit(4);
                    break;
                case Key.D5:
                case Key.NumPad5:
                    calc.AddDigit(5);
                    break;
                case Key.D6:
                case Key.NumPad6:
                    calc.AddDigit(6);
                    break;
                case Key.D7:
                case Key.NumPad7:
                    calc.AddDigit(7);
                    break;
                case Key.D8:
                case Key.NumPad8:
                    calc.AddDigit(8);
                    break;
                case Key.D9:
                case Key.NumPad9:
                    calc.AddDigit(9);
                    break;
                case Key.D0:
                case Key.NumPad0:
                    calc.AddDigit(0);
                    break;
                case Key.Divide:
                    calc.AddOperation(Operation.Div);
                    break;
                case Key.Multiply:
                    calc.AddOperation(Operation.Mul);
                    break;
                case Key.Subtract:
                    calc.AddOperation(Operation.Sub);
                    break;
                case Key.Add:
                    calc.AddOperation(Operation.Add);
                    break;
                case Key.Enter:
                    calc.Compute();
                    break;
            }

        }
    }
}
