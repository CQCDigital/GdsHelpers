using System;
using System.Threading.Tasks;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-footer-container-meta-item", ParentTag = "gds-footer-container-meta")]
    public class FooterMetaItemContainerHelper : TagHelper
    {
        [HtmlAttributeName("additional-styles")]
        [Obsolete("Using the tag's class attribute will add additional css classes")]
        public string AdditionalStyles { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("govuk-footer__meta-item");

            #pragma warning disable CS0618 // Type or member is obsolete
            if (!string.IsNullOrEmpty(AdditionalStyles))
            {
                output.AddClass(AdditionalStyles);
            }
            #pragma warning restore CS0618 // Type or member is obsolete

            var children = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(children);
        }
    }
}