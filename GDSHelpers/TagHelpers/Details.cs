using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-details", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DetailsHelper : TagHelper
    {

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("content")]
        public string Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "details";
            output.AddClass("govuk-details");
            output.Attributes.SetAttribute("data-module", "govuk-details");

            var sb = new StringBuilder();
            sb.AppendLine("<summary class=\"govuk-details__summary\">");
            sb.AppendLine("<span class=\"govuk-details__summary-text\">");
            sb.AppendLine($"{Title}");
            sb.AppendLine("</span>");
            sb.AppendLine("</summary>");

            sb.AppendLine("<div class=\"govuk-details__text\">");
            sb.AppendLine($"{Content}");
            sb.AppendLine("</div>");

            output.PostContent.SetHtmlContent(sb.ToString());
        }
    }
}
