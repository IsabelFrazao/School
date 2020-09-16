using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Entities
{
    public class Schedule : IEntity
    {
        public int Id { get; set; }

        public string Shift { get; set; }
    }
}
