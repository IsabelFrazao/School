using Microsoft.AspNetCore.Mvc.Rendering;
using School.Web.Data;
using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace School.Web.Helpers
{
    public class CombosHelper : IDisposable, ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCourses()
        {
            var list = _context.Courses.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Course...)",
                Value = "0"
            });

            return list;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
