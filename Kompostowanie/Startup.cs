using System.Web.Mvc;
using Kompostowanie.Records;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Kompostowanie.Startup))]
namespace Kompostowanie
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            UnitOfWork.Initialize();

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }
    }
}
