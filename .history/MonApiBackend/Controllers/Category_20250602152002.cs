using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;

namespace MonApiBackend.Controllers
{
    public class Category : Controller
    {
        private readonly AppDbContext _context;
        public Category(AppDbContext context)
        {
            _context = context;
        }

    }
}
