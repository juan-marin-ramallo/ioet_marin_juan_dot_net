using Microsoft.VisualStudio.TestTools.UnitTesting;
using ioet_marin_juan_dot_net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioet_marin_juan_dot_net.Utils.Tests
{
    [TestClass()]
    public class SalaryCalculatorTests
    {
        [TestMethod()]
        public void CalculatePayByEmployeeTest1()
        {
            string line = "RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00";
            double total_to_pay = SalaryCalculator.CalculatePayByEmployee(line);
            Assert.AreEqual(total_to_pay, 215);
        }
    }
}