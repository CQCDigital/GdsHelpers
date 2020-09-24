using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{
    [Obsolete("Use a tag and add gds attribute")]
    [HtmlTargetElement("gds-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LinkHelper : TagHelper
    {
        public LinkHelper()
        {
            AddHiddenText = false;
        }

        [HtmlAttributeName("link-id")]
        public string ListItemId { get; set; } 
         
        [HtmlAttributeName("link-text")]
        public string LinkText { get; set; }

        /// <summary>
        /// Adds the "(opens in new window)" text in a hidden span, this is for accessibility reasons.
        /// </summary>
        [HtmlAttributeName("add-hidden-text")]
        public bool AddHiddenText { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hiddenText = $"<span class=\"govuk-visually-hidden\"> (opens in new window)</span>";

            output.TagName = "a";
            output.Attributes.SetAttribute("id", string.IsNullOrEmpty(ListItemId) ? "" : ListItemId);

            if (AddHiddenText)
                output.PostContent.AppendHtml(hiddenText);
             
            output.Content.SetContent(LinkText);

        }
    }
}
