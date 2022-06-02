using ioet_marin_juan_dot_net.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioet_marin_juan_dot_net.entities
{
    public sealed class Employee
    {
        private Employee() { }

        public String? Name { get; set; }

        public Dictionary<string, DayTimeWorked>? ScheduleWorkDict { get; set; }

        //Get Employee object with name and schedule worked
        public static Employee? GetEmployeeInfo(String employee_job_schedule)
        {
            Employee? employee = null;

            try
            {
                //Left part from = is the name
                String name = employee_job_schedule.Split("=")[0];
                //Create a new dictionary storing day like key and DayTimeWorked object like value.
                //DayTimeWorked contains also day, start time and finish time for each day worked
                Dictionary<string, DayTimeWorked> schedule_worked_dictionary = new Dictionary<string, DayTimeWorked>();

                //Right part from = is the schedule_worked separate with comma ,
                foreach (String day_time_worked_str in employee_job_schedule.Split("=")[1].Split(","))
                {
                    //#Check if day_time_worked length is exactly 13 characters. Example: FR23:30-00:45
                    if (day_time_worked_str.Length != 13)
                        throw new Exception("The length to describe day and time worked is incorrect");

                    //Extract 2 first digits to get the day initials
                    String day_worked = day_time_worked_str.Substring(0, 2);

                    //Extract 11 last digits to get the time range worked
                    String time_worked = day_time_worked_str.Substring(day_time_worked_str.Length - 11);

                    //Get the start time worked from time range
                    DateTime start_time_worked = DateTime.ParseExact(time_worked.Split("-")[0].Trim(), "%H:%m", CultureInfo.InvariantCulture);

                    //Get the end time worked from time range
                    DateTime end_time_worked = DateTime.ParseExact(time_worked.Split("-")[1].Trim(), "%H:%m", CultureInfo.InvariantCulture);

                    //Create an objet Entity from class DayTimeWorked using day, start time and end time worked
                    DayTimeWorked day_time_worked_entity = new DayTimeWorked(day_worked, start_time_worked, end_time_worked);

                    //Add the day_time_worked_entity to the Dictionary
                    schedule_worked_dictionary.Add(day_worked, day_time_worked_entity);
                }

                //Create an object Entity from Employee
                employee = new Employee();
                employee.Name = name;
                employee.ScheduleWorkDict = schedule_worked_dictionary;
            }
            catch (Exception)
            {
                throw;
            }

            return employee;
        }
    }
}
