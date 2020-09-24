using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    /// <summary>
    /// GDS Unordered list - <a href="https://design-system.service.gov.uk/styles/typography/#lists">GDS documentation</a>
    /// </summary>
    [HtmlTargetElement("ul", Attributes = "gds", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class UnorderedListTagHelper : ListTagHelper
    {
        public UnorderedListTagHelper(IConfiguration config) : base(config) { }

        [HtmlAttributeName("gds-bullets")]
        public bool ShowBullets { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (ShowBullets)
            {
                output.AddClass("govuk-list--bullet");
            }
        }
    }
}