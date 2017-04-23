using System.Web.Mvc;
using Kompostowanie.Records;

namespace Kompostowanie.Controllers
{
    public class BaseController : Controller
    {
        protected UnitOfWork UnitOfWork { get; set; }

        public BaseController()
        {
            UnitOfWork = new UnitOfWork();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UnitOfWork.BeginTransaction();

            base.OnActionExecuting(filterContext);
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            UnitOfWork.Commit();
            UnitOfWork.Dispose();

            base.OnResultExecuted(filterContext);
        }
    }
}