using System.Web.Mvc;

namespace OrangeCMS.Application.Controllers
{
    public class ApplicationController : Controller
    {
        [Route("")]
        public ViewResult Home()
        {
            return View();
        }
    }
}
