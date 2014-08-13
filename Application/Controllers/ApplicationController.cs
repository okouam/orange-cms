using System.Web.Mvc;

namespace OrangeCMS.Application
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
