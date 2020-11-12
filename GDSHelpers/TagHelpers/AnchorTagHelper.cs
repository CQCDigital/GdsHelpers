using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    /// <summary>
    /// GDS Anchor - <a href="https://design-system.service.gov.uk/styles/typography/#links">GDS documentation</a>
    /// </summary>
    [HtmlTargetElement("a", Attributes = "gds", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AnchorTagHelper : GdsTagHelper
    {
        private readonly IConfiguration _config;

        public AnchorTagHelper(IConfiguration config) : base(config)
        {
            _config = config;
        }

        /// <summary>
        /// Adds text in a hidden span for accessibility reasons.
        /// </summary>
        [HtmlAttributeName("gds-hidden-text")]
        public string HiddenText { get; set; }

        /// <summary>
        /// Will this link open in a new window
        /// </summary>
        [HtmlAttributeName("gds-new-window")]
        public bool NewWindow { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass("govuk-link");
            AddHiddenContent(HiddenText, output);
            ConfigureNewWindow(NewWindow, output);
        }
        
        private static void AddHiddenContent(string content, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            var hiddenText = $"<span class=\"govuk-visually-hidden\">{content}</span>";
            output.PostContent.AppendHtml(hiddenText);
        }
        
        private void ConfigureNewWindow(bool newWindow, TagHelperOutput output)
        {
            if (!newWindow)
            {
                return;
            }
            output.Attributes.SetAttribute("target", "_blank");
            output.Attributes.SetAttribute("rel", "noreferrer noopener");
            output.PostContent.AppendHtml($" {Settings.Options.AnchorTagNewWindowText}");
        }
    }
}