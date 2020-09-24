using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    /// <summary>
    /// GDS Paragraph - <a href="https://design-system.service.gov.uk/styles/typography/#paragraphs">GDS documentation</a>
    /// </summary>
    [HtmlTargetElement("p", Attributes = "gds", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ParagraphTagHelper : GdsTagHelper
    {
        public ParagraphTagHelper(IConfiguration config) : base(config) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass("govuk-body");
        }
    }
}