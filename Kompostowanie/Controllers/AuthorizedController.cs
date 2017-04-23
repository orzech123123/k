using System.Web.Mvc;
using Kompostowanie.Helpers;

namespace Kompostowanie.Controllers
{
    public class AuthorizedController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHelper.UserSession(Session) == null)
                filterContext.Result = RedirectToAction("Login", "User");

            base.OnActionExecuting(filterContext);
        }
    }
}