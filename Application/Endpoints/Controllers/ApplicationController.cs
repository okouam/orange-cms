using System.Web.Mvc;

namespace CodeKinden.OrangeCMS.Application.Endpoints.Controllers
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
