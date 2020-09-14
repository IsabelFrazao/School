using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace School.Web.Helpers
{
    public interface ICombosHelper
    {
        void Dispose();

        IEnumerable<SelectListItem> GetComboCourses();
    }
}