using Microsoft.VisualStudio.TestTools.UnitTesting;
using ioet_marin_juan_dot_net.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioet_marin_juan_dot_net.entities.Tests
{
    [TestClass()]
    public class EmployeeTests
    {
        [TestMethod()]
        public void GetEmployeeInfoTest()
        {
            string line = "RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00";
            Employee? employee_job_schedule_entity = Employee.GetEmployeeInfo(line);
            Assert.AreEqual(employee_job_schedule_entity!.Name, "RENE");
        }
    }
}