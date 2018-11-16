using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyCalcLibrary
{
    public class MyCalc
    {   
        public int Sum (int x, int y)
        {
            return x + y;
        }
        
    }
}
 
    public static class CalculateExpression
    {
        private static readonly Dictionary<string, byte> _opdict = new Dictionary<string, byte>
            {{"(", 0}, {"+", 1}, {"-", 2},{"/", 3}, {"*", 4}};

        private static readonly HashSet<string> _operators = new HashSet<string> { "+", "-", "/", "*" };

        private static string _input;

        private static double CalculateSimple(double a, double b, char op)
        //Выполняем нужное вычисление в зависимости от операции
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
            }
            return 0;
        }

        private static double StackCalc(double a, double b, char op)
        //Так как из стека достаем в обратном порядке, требуется инвертировать левый и правый операнды
        {
            return CalculateSimple(b, a, op);
        }

        private static bool BadBreckets(string s) //Проверяем баланс скобок в строке
        {
            int i = 0;
            foreach (char c in s)
            {
                if (c == '(')
                    i++;
                else if (c == ')')
                    i--;
            }
            return i != 0;
        }

        private static string Reverse(string s) //Обращаем строку в обратный порядок
        {
            int length = s.Length;
            var sb = new StringBuilder(length, length);
            for (int i = length - 1; i >= 0; i--)
                sb.Append(s[i]);
            return sb.ToString();
        }

        public static string ToRPN() //Для более удобного анализа переводим в ОПН
        {
            var result = new StringBuilder(_input.Length);
            bool negative = false;
            var stack = new Stack<char>();
            foreach (char c in _input)
            {
                if (c == ' ') continue;
                if (c == '(')
                    stack.Push(c);
                else if (c == ')')
                //Если встречаем закрывающую скобку, то вытряхиваем в строку все, до появления открывабщей скобки
                {
                    while (stack.Count > 0)
                    {
                        char a = stack.Pop();
                        if (a == '(') break;
                        result.Append(" " + a);
                    }
                }
                else if (_operators.Contains(c.ToString()))
                //Иначе если это оператор, вытряхиваем все операторы с большим приоритетом в строку, после этого засовываемся сами в стек
                {
                    string op = c.ToString();
                    if (c == '-')
                    {
                        negative = true;
                        result.Append("0 "); //Используем математический подход a - b = a + (0 - b)
                        op = "+";
                    }
                    else
                        negative = false;
                    result.Append(" ");
                    while (stack.Count > 0 && _opdict[op] < _opdict[stack.Peek().ToString()])
                    {
                        result.Append(stack.Pop() + " ");
                    }
                    stack.Push(op[0]);
                }
                else
                {
                    if (negative)
                    {
                        result.Append('-');
                        negative = false;
                    }
                    result.Append(c);
                } //Если это не скобки и не операторы, то это цифра, добавляем к выходной строке, с учетом знака
            }
            while (stack.Count > 0) //Вытряхиваем в строку оставшиеся символы
                result.Append(" " + stack.Pop());
            return result.ToString();
        }

        public static string ToPN()
        {
            return Reverse(ToRPN());
        }

        public static double Calculate(string input)
        //Вычисляем выражение. Если формат ввода неправильный, возвращаем null
        {
            if (BadBreckets(input) || String.IsNullOrEmpty(input))
                return double.NaN;
            _input = input;
            string[] strings = ToRPN().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var stack = new Stack<double>(input.Length / 2);
            foreach (string s in strings)
            {
                if (_opdict.ContainsKey(s)) //Если оператор, то выполняем его над последними двумя элементами стека.
                {
                    if (stack.Count < 2)
                        if (s == "+" || s == "-")
                        {
                            stack.Push(StackCalc(stack.Pop(), 0, s[0]));
                            continue;
                        }
                        else return double.NaN;
                    stack.Push(StackCalc(stack.Pop(), stack.Pop(), s[0]));
                }
                else //Иначе это должно быть число, если вдруг что-то другое, то возвращаем null
                {
                    double a;
                    if (!double.TryParse(s, out a)) return double.NaN;
                    stack.Push(a);
                }
            }
            return Math.Round(stack.Peek(), 15);
            //В стеке остается единственный элемент, значение выражения, который возвращаем
        }
    }

