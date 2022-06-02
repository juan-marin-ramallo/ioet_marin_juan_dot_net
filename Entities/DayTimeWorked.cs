using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioet_marin_juan_dot_net.entities
{
    public class DayTimeWorked
    {
        private String _day_worked;
        private DateTime _start_time_worked;
        private DateTime _end_time_worked;

        public DayTimeWorked(String day_worked, DateTime start_time_worked, DateTime end_time_worked)
        {
            _day_worked = day_worked;
            _start_time_worked = start_time_worked;
            _end_time_worked = end_time_worked;
        }

        public String DayWorked { get => _day_worked; set => _day_worked = value; }
        public DateTime StartTimeWorked { get => _start_time_worked; set => _start_time_worked = value; }
        public DateTime EndTimeWorked { get => _end_time_worked; set => _end_time_worked = value; }
    }
}
