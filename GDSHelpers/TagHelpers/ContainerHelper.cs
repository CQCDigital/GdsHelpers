using Microsoft.AspNetCore.Razor.TagHelpers;
using GDSHelpers.Extensions;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-container")]
    public class ContainerHelper : TagHelper
    {
  
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("govuk-width-container");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
