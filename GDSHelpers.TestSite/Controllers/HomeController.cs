using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

namespace GDSHelpers.TestSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}