using System.Text.Encodings.Web;
using GDSHelpers.Enums;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    /// <summary>
    /// GDS Heading - <a href="https://design-system.service.gov.uk/styles/typography/#headings">GDS documentation</a>
    /// </summary>
    [HtmlTargetElement("h1", Attributes = "gds")]
    [HtmlTargetElement("h2", Attributes = "gds")]
    [HtmlTargetElement("h3", Attributes = "gds")]
    [HtmlTargetElement("h4", Attributes = "gds")]
    [HtmlTargetElement("h5", Attributes = "gds")]
    [HtmlTargetElement("h6", Attributes = "gds")]
    public class HeadingTagHelper : GdsTagHelper
    {
        public HeadingTagHelper(IConfiguration config) : base(config) { }

        /// <summary>
        /// Override the default size of header by specifying a GdsSize
        /// </summary>
        [HtmlAttributeName("gds-size")]
        public GdsSize? Size { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var className = GetClassFor(output.TagName);
            output.AddClass(className, HtmlEncoder.Default);
        }

        private string GetClassFor(string headerType)
        {
            var size = (Size ?? GetDefaultSize()).ToClassString();

            return $"govuk-heading-{size}";

            GdsSize GetDefaultSize()
            {
                switch (headerType.ToLower())
                {
                    case "h1": return GdsSize.ExtraLarge;
                    case "h2": return GdsSize.Large;
                    case "h3": return GdsSize.Medium;
                    default: return GdsSize.Small;
                }
            }
        }
    }
}