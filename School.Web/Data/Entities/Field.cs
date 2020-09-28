using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Entities
{
    public class Field : IEntity
    {
        public int Id { get; set; }

        public string Area { get; set; }

        public bool isActive { get; set; }
    }
}
