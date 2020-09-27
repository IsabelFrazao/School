using School.Web.Data.Entities;
using System.Collections.Generic;

namespace School.Web.Models
{
    public class SettingsViewModel
    {
        public IEnumerable<Schedule> Schedules { get; set; }

        public Schedule Schedule { get; set; }

        public IEnumerable<Classroom> Classrooms { get; set; }

        public Classroom Classroom { get; set; }
    }
}
