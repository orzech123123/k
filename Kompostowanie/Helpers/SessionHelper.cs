using System.Web;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Helpers
{
    public class SessionHelper
    {
        public static UserSession UserSession(HttpSessionStateBase session)
        {
            return (UserSession)session["UserSession"];
        }
        
        public static void SetUserSession(HttpSessionStateBase session, UserSession user)
        {
            session["UserSession"] = user;
        }
    }
}