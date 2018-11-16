using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCalcLibrary
{
    [TestClass]
    public class MyCalcTests
    {
        [TestMethod]
        public void Sum_10and20_30return()
        {
            // arrange (подготовка полей, переменных)
            int x = 10;
            int y = 20;
            int rezult = 30;

            // act (действие, функция)
            MyCalc obj = new MyCalc();
            int actual = obj.Sum(x, y);

            // assert (метод сравнения)
            Assert.AreEqual(actual, rezult, "Результат тестирования суммы");
        }
    }
}
