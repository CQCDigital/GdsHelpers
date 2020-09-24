using System.Threading.Tasks;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-header-container", ParentTag = "gds-header")]
    public class HeaderContainerHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("govuk-header__container");
            output.AddClass("govuk-width-container");

            var children = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(children);
        }
    }
}