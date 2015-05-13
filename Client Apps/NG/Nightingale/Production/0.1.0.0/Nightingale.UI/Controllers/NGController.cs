using System.Web.Mvc;

namespace Nightingale.Site.Controllers
{

  public class NGController : Controller {
    [Authorize]
    public ActionResult Index() {
        return View();
    }

    [Authorize]
    public ActionResult LogOff()
    {
        System.Web.Security.FormsAuthentication.SignOut();

        return RedirectToAction("Index", "Home");
    }
  }
}