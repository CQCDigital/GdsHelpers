using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    /// <summary>
    /// GDS Ordered list - <a href="https://design-system.service.gov.uk/styles/typography/#lists">GDS documentation</a>
    /// </summary>
    [HtmlTargetElement("ol", Attributes = "gds", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class OrderedListTagHelper : ListTagHelper
    {
        public OrderedListTagHelper(IConfiguration config) : base(config) { }

        /// <summary>
        /// Use when you want the html markup of an ordered list, but don't want to use the standard browser numbering
        /// </summary>
        [HtmlAttributeName("gds-hide-numbers")]
        public bool HideNumbers { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (!HideNumbers)
            {
                output.AddClass("govuk-list--number");
            }
        }
    }
}