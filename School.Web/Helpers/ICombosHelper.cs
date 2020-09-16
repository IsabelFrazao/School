using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace School.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboCourses();

        void Dispose();
    }
}