using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    public abstract class ListTagHelper : GdsTagHelper
    {
        public ListTagHelper(IConfiguration config) : base(config) { }

        /// <summary>
        /// If a list is hard to read because the items run across multiple lines you can add extra spacing.
        /// (Supported in GDS version 3.7)
        /// </summary>
        [HtmlAttributeName("gds-extra-spacing")]
        public bool ExtraSpacing { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass("govuk-list");
            
            if (ExtraSpacing)
            {
                output.AddClass("govuk-list--spaced");
            }
        }
    }
}