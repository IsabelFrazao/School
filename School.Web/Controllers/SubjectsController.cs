using Microsoft.AspNetCore.Mvc;

namespace School.Web.Controllers
{
    public class SubjectsController : Controller
    {
        // GET: CRUDController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CRUDController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
