using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kompostowanie.Extensions;

namespace Kompostowanie.Helpers
{
    public static class EnumDropDownListHelper
    {
        public static MvcHtmlString EnumDropDownListForWithDescription<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListForWithDescription(htmlHelper, expression, null);
        }

        public static MvcHtmlString EnumDropDownListForWithDescription<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items = from value in values
                        select new SelectListItem
                        {
                            Text = EnumExtensions.GetEnumDescription(value),
                            Value = value.ToString(),
                            Selected = value.Equals(metadata.Model)
                        };

            if (metadata.IsNullableValueType)
                items = SingleEmptyItem.Concat(items);

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }


        public static MvcHtmlString EnumDropDownListWithDescription<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, TEnum enumek)
        {
            return EnumDropDownListWithDescription(htmlHelper, enumek, null);
        }

        public static MvcHtmlString EnumDropDownListWithDescription<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, TEnum enumek, object htmlAttributes)
        {
            var values = Enum.GetValues(enumek.GetType()).Cast<TEnum>();

            var items = values.Select(value => new SelectListItem
            {
                Text = EnumExtensions.GetEnumDescription(value),
                Value = value.ToString()
            });

            return htmlHelper.DropDownList(Guid.NewGuid().ToString(), items, htmlAttributes ?? new {});
        }


        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            var realModelType = modelMetadata.ModelType;

            var underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }

            return realModelType;
        }

        private static readonly SelectListItem[] SingleEmptyItem = { new SelectListItem { Text = "", Value = "" } };
    }
}