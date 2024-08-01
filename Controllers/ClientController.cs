using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult ClientDashBoard()
        {
            return View();
        }
    }
}
