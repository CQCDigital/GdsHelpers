using System.Data;
using GDSHelpers.Extensions;
using GDSHelpers.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text;

namespace GDSHelpers.TagHelpers
{
    [HtmlTargetElement("gds-bread-crumbs")]
    public class BreadcrumbsHelper : TagHelper
    {
        [HtmlAttributeName("bread-crumbs")]
        public BreadCrumbs Breadcrumbs { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string GenerateLink(Crumb crumb)
            {
                crumb ??= new Crumb("", null);
                string href = (crumb.Url != null) ? $"href=\"{crumb.Url}\"" : "";
                return $"<a class=\"govuk-breadcrumbs__link govuk-link--no-visited-state\" {href}\">{crumb.Text}</a>";
            }

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.AddClass("govuk-breadcrumbs");

            var sb = new StringBuilder();
            sb.AppendLine("<ol class=\"govuk-breadcrumbs__list\">");

            var last = Breadcrumbs.Crumbs.Last();

            foreach (var crumb in Breadcrumbs.Crumbs)
            {
                string ariaCurrent = "";
                if (crumb.Equals(last)) //last crumb
                {
                    ariaCurrent = "aria-current=\"page\"";
                    crumb.Url = null; //no href on last element
                }

                sb.AppendLine($"<li class=\"govuk-breadcrumbs__list-item\" {ariaCurrent}>");
                sb.AppendLine(GenerateLink(crumb));
                sb.AppendLine("</li>");
            }

            sb.AppendLine("</ol>");

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
