using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
using m

namespace MonApiBackend.Controllers
{
    public class Category : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

    }
}
