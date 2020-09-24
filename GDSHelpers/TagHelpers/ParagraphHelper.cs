using System;
using System.Threading.Tasks;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{
    [Obsolete("Use p tag and add gds attribute")]
    [HtmlTargetElement("gds-paragraph", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ParagraphHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "p";
            output.AddClass("govuk-body");
        }
    }
}