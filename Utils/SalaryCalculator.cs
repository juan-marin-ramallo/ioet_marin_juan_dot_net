using ioet_marin_juan_dot_net.entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioet_marin_juan_dot_net.Utils
{
    public class SalaryCalculator
    {
        public static Double CalculatePayByEmployee(String employee_job_schedule)
        {
            double total_to_pay = 0.0;

            try
            {
                Employee? employee_entity = Employee.GetEmployeeInfo(employee_job_schedule);                
                DateTime iterate_time_worked;
                Dictionary<String, String>? work_hour_salaries_by_day;

                foreach (DayTimeWorked day_time_worked_entity in employee_entity?.ScheduleWorkDict!.Values!)
                {
                    //Get the dictionary with work hour salaries according to day_worked
                    work_hour_salaries_by_day = GetWorkHourSalariesDictByDay(day_time_worked_entity.DayWorked);

                    //Check if exists a schedule for the input value entered in DayWorked
                    if (work_hour_salaries_by_day == null)
                        throw new Exception("The day " + day_time_worked_entity.DayWorked + " is not a valid day. Please check input data");

                    //Add 1 minute to make match with start time from work_hour_salaries
                    if (day_time_worked_entity.StartTimeWorked.Minute == 0)
                        iterate_time_worked = day_time_worked_entity.StartTimeWorked.AddMinutes(1);
                    else
                        iterate_time_worked = day_time_worked_entity.StartTimeWorked;

                    //Add 1 day to end_time_worked if it is equal to zero
                    if (day_time_worked_entity.EndTimeWorked.Hour == 0)
                        day_time_worked_entity.EndTimeWorked = day_time_worked_entity.EndTimeWorked.AddDays(1);

                    //Iterate every hour worked for each day_worked
                    while (iterate_time_worked < day_time_worked_entity.EndTimeWorked)
                    {
                        Double amount_to_pay_by_hour = GetAmountToPayByHour(iterate_time_worked, work_hour_salaries_by_day);

                        if (amount_to_pay_by_hour > 0)
                            total_to_pay = total_to_pay + amount_to_pay_by_hour;

                        //Add 1 hora to iterate_time_worked to continue to the next hour worked
                        iterate_time_worked = iterate_time_worked.AddHours(1);
                    }

                    //Validate if the minute of end_time_worked is greater than 0 to pay extra hour
                    if (day_time_worked_entity.EndTimeWorked.Minute > 0)
                    {
                        iterate_time_worked = day_time_worked_entity.EndTimeWorked;

                        //Validate if the hour is equal 0 to get the next day and the work_hour_salaries for this next day
                        if (day_time_worked_entity.EndTimeWorked.Hour == 0)
                        {
                            iterate_time_worked = day_time_worked_entity.EndTimeWorked.AddDays(-1);
                            String next_day_worked = GetNextDayWorked(day_time_worked_entity.DayWorked);
                            work_hour_salaries_by_day = GetWorkHourSalariesDictByDay(next_day_worked);
                        }

                        //Calculate the amount to pay according to iterate_time_worked and work_hour_salaries
                        Double amount_to_pay_by_hour = GetAmountToPayByHour(iterate_time_worked, work_hour_salaries_by_day);

                        if (amount_to_pay_by_hour > 0)
                            total_to_pay = total_to_pay + amount_to_pay_by_hour;
                    }
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return total_to_pay;
        }

        //Read json file with week and weekend hour salaries using the json decode method load
        private static Dictionary<String, Dictionary<String, String>>? GetCompleteWorkHourSalariesDict() {
            Dictionary<String, Dictionary<String, String>>? work_hour_salaries = null;

            try
            {
                //Opening Work Hour Salaries JSON file
                using (StreamReader r = new StreamReader(@"..\..\..\Data\work_hour_salaries.json"))
                {
                    string jsonWorkHourSalaries = r.ReadToEnd();
                    //Converts json file to a Dictionary<String, Dictionary<String, String>>
                    work_hour_salaries = JsonConvert.DeserializeObject<Dictionary<String, Dictionary<String, String>>>(jsonWorkHourSalaries);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return work_hour_salaries;
        }

        //This Function return work hour salaries dictionary based on week or weekend day worked.
        //The Parameter received is a day with its 2 first initials
        private static Dictionary<String, String>? GetWorkHourSalariesDictByDay(String day_worked) {
            Dictionary<String, String>? work_hour_salaries_by_day = null;

            Dictionary<String, Dictionary<String, String>>? complete_work_hour_salaries = GetCompleteWorkHourSalariesDict();

            //Depending the day_worked by employee return a Dictionary<String, String> with the schedules and payments respectively
            if (day_worked == "SA" || day_worked == "SU")
                work_hour_salaries_by_day = complete_work_hour_salaries!["weekend"];
            else if (day_worked == "MO" || day_worked == "TU" || day_worked == "WE" || day_worked == "TH" || day_worked == "FR")
                work_hour_salaries_by_day = complete_work_hour_salaries!["week"];
            
            return work_hour_salaries_by_day;
        }

        //This function return then amount to pay for every hour worked on base to time
        //worked and to work hour salaries dictionary according if a week day o weekend day
        private static Double GetAmountToPayByHour(DateTime iterate_time_worked, Dictionary<String, String>? work_hour_salaries) { 
            Double amount_to_pay_by_hour = 0;

            try
            {
                //Iterate every schedule existing in work_hour_salaries
                foreach (KeyValuePair<String, String> entry in work_hour_salaries!)
                {
                    DateTime start_time_schedule = DateTime.ParseExact(entry.Key.Split("-")[0].Trim(), "%H:%m", CultureInfo.InvariantCulture);
                    DateTime end_time_schedule = DateTime.ParseExact(entry.Key.Split("-")[1].Trim(), "%H:%m", CultureInfo.InvariantCulture);

                    //Add 1 day is the end_time_schedule is 0
                    if (end_time_schedule.Hour == 0)
                        end_time_schedule = end_time_schedule.AddDays(1);

                    //Try to find how many to pay by hour worked
                    if (iterate_time_worked >= start_time_schedule && iterate_time_worked < end_time_schedule)
                    {
                        amount_to_pay_by_hour = Double.Parse(entry.Value.Split(" ")[0]);
                        break;
                    }                        
                }
            }
            catch (Exception)
            {
                throw;
            }

            return amount_to_pay_by_hour;
        }

        //This Function returns the next day of the week or weekend.
        //The Parameter received is a day with its 2 first initials
        private static String GetNextDayWorked(String day_worked) {
            if (day_worked == "SU")
                return "MO";
            if (day_worked == "MO")
                return "TU";
            if (day_worked == "TU")
                return "WE";
            if (day_worked == "WE")
                return "TH";
            if (day_worked == "TH")
                return "FR";
            if (day_worked == "FR")
                return "SA";
            if (day_worked == "SA")
                return "SU";

            return "";
        }
    }
}
