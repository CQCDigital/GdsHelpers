﻿using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-notification-text", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NotificationHelper : TagHelper
    {
        
        [HtmlAttributeName("notification-title")]
        public string Title { get; set; }

        [HtmlAttributeName("message")]
        public string Message { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "govuk-notification-banner");
            output.Attributes.SetAttribute("id", "div_notification_banner");
           
            var htmlTitle = $"<h2 class=\"govuk-notification-banner__title\" id=\"notification-banner-title\">{Title}</h2>";
            
            var sb = new StringBuilder();
            sb.AppendLine(string.IsNullOrWhiteSpace(Title)? "" : htmlTitle);
            sb.AppendLine("<div class=\"govuk-notification-banner__body\">");
            sb.AppendLine($"<p>{Message}</p>");
            sb.AppendLine("</div>");
            output.PostContent.SetHtmlContent(sb.ToString());
        }
    }
}
 