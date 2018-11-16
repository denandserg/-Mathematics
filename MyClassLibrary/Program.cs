using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    class Program
    {

    }
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Введите математическое  выражение для вычисления. Для выхода напишите exit");
            string s = Console.ReadLine();
            if (s == "exit")
                return;
            double result = CalculateExpression.Calculate(s);
            Console.WriteLine(double.IsNaN(result) ? "Неправильный формат ввода" : $"{result:0.000}");
        }
    }
}
