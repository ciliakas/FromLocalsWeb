using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace FromLocalsToLocals.Utilities
{
    public static class HtmlDropDownExtensions
    {
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name,
            TEnum selectedValue)
        {
            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();

            var items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    Selected = value.Equals(selectedValue)
                };

            return htmlHelper.DropDownList(
                name,
                items
            );
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            var newListOfEnumValues =
                enumValues.Select(enumMember => new SelectListItem
                {
                    Text = enumMember.ToString(),
                    Value = enumMember.ToString(),
                    Selected = enumMember.Equals(metadata.Model)
                });

            return htmlHelper.DropDownListFor(
                expression,
                newListOfEnumValues
            );
        }
    }
}