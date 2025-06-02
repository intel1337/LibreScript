using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;

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
