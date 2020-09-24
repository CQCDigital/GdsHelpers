using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{
    [HtmlTargetElement("gds-table", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GdsTableTagHelper : TagHelper
    {
        [HtmlAttributeName("items")]
        public IEnumerable<object> Items { get; set; }

        [HtmlAttributeName("caption")]
        public string Caption { get; set; }

        [HtmlAttributeName("bold-first-column")]
        public bool BoldFirstColumn { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            output.Attributes.Add("class", "govuk-table");
            var props = GetItemProperties();

            TableCaption(output, Caption);
            TableHeader(output, props);
            TableBody(output, props);
        }

        private void TableCaption(TagHelperOutput output, string Caption)
        {
            if (!string.IsNullOrEmpty(Caption))
            {
                output.Content.AppendHtml($"<caption class=\"govuk-table__caption\">{Caption}</caption>");
            }
        }

        private void TableHeader(TagHelperOutput output, PropertyInfo[] props)
        {
            output.Content.AppendHtml("<thead class=\"govuk-table__head\">");
            output.Content.AppendHtml("<tr class=\"govuk-table__row\">");
            foreach (var prop in props)
            {
                var name = GetPropertyName(prop);
                var intClass = prop.PropertyType.FullName == "System.Int32" ? "govuk-table__header--numeric" : "";
                output.Content.AppendHtml($"<th scope=\"col\" class=\"govuk-table__header {intClass}\">{name}</th>");
            }
            output.Content.AppendHtml("</tr>");
            output.Content.AppendHtml("</thead>");
        }

        private void TableBody(TagHelperOutput output, PropertyInfo[] props)
        {
            output.Content.AppendHtml("<tbody class=\"govuk-table__body\">");
            foreach (var item in Items)
            {
                output.Content.AppendHtml("<tr scope=\"row\" class=\"govuk-table__row\">");
                foreach (var prop in props)
                {
                    var value = GetPropertyValue(prop, item);
                    var intClass = prop.PropertyType.FullName == "System.Int32" ? "govuk-table__cell--numeric" : "";
                    var boldCell = props.First() == prop && BoldFirstColumn ? "govuk-table__header" : "";

                    output.Content.AppendHtml($"<td class=\"govuk-table__cell {intClass} {boldCell}\">{value}</td>");
                }
                output.Content.AppendHtml("</tr>");
            }
            output.Content.AppendHtml("</tbody>");
        }

        private PropertyInfo[] GetItemProperties()
        {
            var listType = Items.GetType();
            if (listType.IsGenericType)
            {
                var itemType = listType.GetGenericArguments().First();
                return itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            return new PropertyInfo[] { };
        }

        private string GetPropertyName(MemberInfo property)
        {
            var attribute = property.GetCustomAttribute<DisplayNameAttribute>();
            if (attribute != null)
            {
                return attribute.DisplayName;
            }
            return property.Name;
        }

        private object GetPropertyValue(PropertyInfo property, object instance)
        {
            return property.GetValue(instance);
        }
    }
}
