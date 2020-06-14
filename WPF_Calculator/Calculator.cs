using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCalc
{
    enum Operation { Add, Div, Sub, Mul, Sin, Sqrt, Pow, Cos, Ln, Log10 }

    delegate void CalculatorDidUpdateOutput(Calculator sender, double value, int precision);

    class Calculator
    {
        double? left = null;
        double? right = null;
        Operation? currentOp = null;
        bool decimalPoint = false;
        int precision = 0;

        public event CalculatorDidUpdateOutput DidUpdateValue;
        public event EventHandler<string> InputError;
        public event EventHandler<string> CalculationError;

        public void AddDigit(int digit)
        {
            if (left.HasValue && Math.Log10(left.Value) > 10 || left.HasValue && Math.Log10(left.Value * Math.Pow(10, precision)) > 10)
            {
                InputError?.Invoke(this, "Input overflow");
                return;
            }

            if (!decimalPoint)
            {
                left = (left ?? 0) * 10 + digit;
            }
            else
            {
                precision += 1;
                left = (left ?? 0) + digit / Math.Pow(10, precision);
            }

            DidUpdateValue?.Invoke(this, left.Value, precision);
        }

        public void AddDecimalPoint()
        {
            decimalPoint = true;
        }

        public void AddOperation(Operation op)
        {
            if (left.HasValue && currentOp.HasValue)
            {
                Compute();
            }
            if (!right.HasValue)
            {
                right = left;
                left = 0;
                precision = 0;
                decimalPoint = false;
                DidUpdateValue.Invoke(this, left.Value, precision);
            }
            currentOp = op;
        }

        public void Compute()
        {
            switch (currentOp)
            {
                case Operation.Add:
                    right = left + right;
                    left = null;
                    break;
                case Operation.Sub:
                    right = right - left;
                    left = null;
                    break;
                case Operation.Mul:
                    right = left * right;
                    left = null;
                    break;
                case Operation.Div:
                    if (left == 0)
                    {
                        CalculationError?.Invoke(this, "Division by 0!");
                        return;
                    }
                    right = right / left;
                    left = null;
                    break;
                case Operation.Sin:
                    right = Math.Sin(right.Value);
                    left = null;
                    break;
                case Operation.Sqrt:
                    if (right < 0)
                    {
                        CalculationError?.Invoke(this, "Error");
                        return;
                    }
                    else right = Math.Sqrt(right.Value);
                    left = null;
                    break;
                case Operation.Cos:
                    right = Math.Cos(right.Value);
                    left = null;
                    break;
                case Operation.Ln:
                    if (right <= 0)
                    {
                        CalculationError?.Invoke(this, "Error");
                        return;
                    }
                    right = Math.Log(right.Value);
                    left = null;
                    break;
                case Operation.Log10:
                    if (right <= 0)
                    {
                        CalculationError?.Invoke(this, "Error");
                        return;
                    }
                    right = Math.Log10(right.Value);
                    left = null;
                    break;
                case Operation.Pow:
                    right = Math.Pow(right.Value, left.Value);
                    left = null;
                    break;

            }
            DidUpdateValue?.Invoke(this, right.Value, precision);
            precision = 0;
            decimalPoint = false;
        }

        public void ClearSymbol()
        {
            if (decimalPoint)
            {
                left = left - (left * Math.Pow(10, precision - 1) % 1) * Math.Pow(0.1, precision - 1);
                precision--;
                if (precision == 0)
                    decimalPoint = false;
            }
            else
            {
                left = (int)(left * 0.1);
            }
            DidUpdateValue?.Invoke(this, left.Value, precision);
        }

        public void Reset()
        {
            currentOp = null;
            left = 0;
            right = null;
            precision = 0;
            decimalPoint = false;
            DidUpdateValue?.Invoke(this, left.Value, 0);
        }

    }
}
