using System.Collections.Generic;

namespace School.Web.Data.Entities
{
    public class Classroom : IEntity
    {
        public int Id { get; set; }

        public string Room { get; set; }

        public bool isActive { get; set; }
    }
}
