using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-footer-container", ParentTag = "gds-footer")]
    public class FooterContainerHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("govuk-footer__container");
            output.AddClass("govuk-width-container");

            var children = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(children);
        }
    }

}
