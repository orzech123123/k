using System.Globalization;
using System.Web.Mvc;

namespace Kompostowanie
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return valueProviderResult == null ? base.BindModel(controllerContext, bindingContext) : decimal.Parse(valueProviderResult.AttemptedValue, CultureInfo.InvariantCulture);
        }
    }
}