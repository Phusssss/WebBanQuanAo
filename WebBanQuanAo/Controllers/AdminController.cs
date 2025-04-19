using Microsoft.AspNetCore.Mvc;

namespace WebBanQuanAo.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
