using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "gds", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ButtonTagHelper : GdsTagHelper
    {
        public ButtonTagHelper(IConfiguration config) : base(config)
        {
        }

        [HtmlAttributeName("gds-start")]
        public bool IsStart { get; set; }

        [HtmlAttributeName("gds-secondary")]
        public bool IsSecondary { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass("govuk-button");
            output.Attributes.Add("data-module", "govuk-button");

            if (IsStart)
            {
                output.AddClass("govuk-button--start");
                if (Settings.GdsVersion >= 3m)
                {
                    output.PostContent.AppendHtml(
                        "<svg class=\"govuk-button__start-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"17.5\" height=\"19\" viewBox=\"0 0 33 40\" aria-hidden=\"true\" focusable=\"false\"><path fill=\"currentColor\" d=\"M0 0h13l20 20-20 20H0l20-20z\" /></svg>");
                }
            }

            if (IsSecondary)
            {
                output.AddClass("govuk-button--secondary");
            }

            if (output.Attributes.ContainsName("disabled"))
            {
                output.Attributes.SetAttribute("aria-disabled", "true");
                output.AddClass("govuk-button--disabled");
            }
        }
    }
}