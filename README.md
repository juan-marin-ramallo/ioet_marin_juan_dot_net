# ioet_marin_juan_dot_net

OVERVIEW OF SOLUTION
My solution solves 2 problems:

* Output a table containing pairs of employees and how often they have coincided in the office. (main excercise)
* Calculate salaries from each employee. (extra excercise)

Also includes unit testing classes and methods.


EXPLANATION OF THE ARCHITECTURE:
I started reading input file "job_schedule.txt" using System.IO.File class, where each line represents a employe and the schedule they worked, indicating the time and hours. Next I created a Employee object for each line readed from file and store each object in a collection using pattern design Iterator. Each employee contains properties like a name and a Dictionary<string, DayTimeWorked>, where DayTimeWorked is another class that represents a day worked with its start time and finish time.

To calculate salary (extra excercise) I used a table of work hour salaries in a json file (not hardcode). For read json file I used a Newtonsoft.Json package. Weekdays are correctly validated. Edge cases works correctly and minutes are managed if the time past 12h01 considering like extra hour for example.

To calculate employees have coincided in the office in the same time frame, I created a IEnumarable collection to store a Tuple with pairs of employees of all possible combinations using Linq. For each combination of pair of employees I compared the day and time worked.

INSTRUCTIONS HOW TO RUN THE PROGRAM LOCALLY:

Just download the project and open solution file called ioet_marin_juan_dot_net.sln in a Visual Studio 2022 IDE.

Thanks.

Juan Marin
