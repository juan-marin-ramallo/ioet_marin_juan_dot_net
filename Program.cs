using ioet_marin_juan_dot_net.Utils;
using ioet_marin_juan_dot_net.entities;

//*******************************************************************************
//Calculate pairs of employees have been at the office within the same time frame
//*******************************************************************************

List<Employee> employees_job_schedule_list = new List<Employee>();
string[] week_days = new string[7] { "MO", "TU", "WE", "TH", "FR", "SA", "SU"};

try
{
    //Create an object employee with schedule info from every line in textfile 
    foreach (string line in File.ReadLines(@"..\..\..\Data\job_schedule.txt"))
    {        
        Employee? employee_job_schedule_entity = Employee.GetEmployeeInfo(line);

        //Add the employee to a list
        employees_job_schedule_list.Add(employee_job_schedule_entity!);

        //Salary Calculator by Employee. Same test I did for Ioet in Python migrated to .NET with C#
        Console.WriteLine(String.Format("The amount to pay {0} is: {1} USD", employee_job_schedule_entity!.Name, SalaryCalculator.CalculatePayByEmployee(line)));
    }

    //Create an IEnumerable to store tuples with pairs of employees of all possible combinations
    IEnumerable<Tuple<Employee,Employee>> pairs_employees_job_schedule =    from i in Enumerable.Range(0, employees_job_schedule_list.Count - 1)
                                                                            from j in Enumerable.Range(i + 1, employees_job_schedule_list.Count - i - 1)
                                                                            select Tuple.Create(employees_job_schedule_list[i], employees_job_schedule_list[j]);

    //For each combination from pair of employees compare if they have coincided in the office
    foreach (Tuple<Employee, Employee> employees_job_schedule_tuple in pairs_employees_job_schedule)
    {
        Employee employee_a = employees_job_schedule_tuple.Item1;
        Employee employee_b = employees_job_schedule_tuple.Item2;
        int pairs_employees_coincidences = 0;

        //Check every week day if the pairs of employees are within the same time frame
        foreach (string day in week_days)
        {
            //If the employee A or B works in the same day of the week, I have to compare the time
            if (employee_a.ScheduleWorkDict!.ContainsKey(day) && employee_b.ScheduleWorkDict!.ContainsKey(day)) 
            {
                //Get the time worked for every day for each employee
                DayTimeWorked day_time_worked_employee_a = employee_a.ScheduleWorkDict![day];
                DayTimeWorked day_time_worked_employee_b = employee_b.ScheduleWorkDict![day];

                //Compare the time for the same day that both employees worked.
                if ((day_time_worked_employee_a.StartTimeWorked >= day_time_worked_employee_b.StartTimeWorked &&
                    day_time_worked_employee_a.EndTimeWorked <= day_time_worked_employee_b.EndTimeWorked) ||
                    (day_time_worked_employee_b.StartTimeWorked >= day_time_worked_employee_a.StartTimeWorked &&
                    day_time_worked_employee_b.EndTimeWorked <= day_time_worked_employee_a.EndTimeWorked))
                    pairs_employees_coincidences++;
            }
        }

        string resultComparasion = String.Format("{0}-{1}: {2}", employee_a.Name, employee_b.Name, pairs_employees_coincidences);
        Console.WriteLine(resultComparasion);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


    
