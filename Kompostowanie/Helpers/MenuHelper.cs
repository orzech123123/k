using System.Web;

namespace Kompostowanie.Helpers
{
    public static class MenuHelper
    {
        public static IHtmlString FillActiveClass(Action action)
        {
            var active = new HtmlString("active");
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;

            var controllerName = routeData.Values["controller"].ToString();

            if (action == Action.Doswiadczenie && controllerName != "LoremIpsum")
            {
                return active;
            }

            var actionName = routeData.Values["action"].ToString();

            if (action == Action.About && actionName == "About"  && controllerName == "LoremIpsum")
            {
                return active;
            }

            if (action == Action.Contact && actionName == "Contact" && controllerName == "LoremIpsum")
            {
                return active;
            }

            return new HtmlString(string.Empty);
        }
    }

    public enum Action
    {
        Doswiadczenie,
        About,
        Contact
    }
}