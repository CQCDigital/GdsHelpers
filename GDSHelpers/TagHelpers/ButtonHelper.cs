using System;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

    [Obsolete("Use button tag and add gds attribute")]
    [HtmlTargetElement("gds-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ButtonHelper : TagHelper
    {
        public ButtonHelper()
        {
            ButtonStatus = GdsEnums.Status.Enabled;
        }

        [HtmlAttributeName("button-id")]
        public string ButtonId { get; set; }

        [HtmlAttributeName("button-type")]
        public GdsEnums.Buttons ButtonType { get; set; }

        [HtmlAttributeName("button-text")]
        public string ButtonText { get; set; }
        
        [HtmlAttributeName("button-status")]
        public GdsEnums.Status ButtonStatus { get; set; }

        [HtmlAttributeName("start-now")]
        public bool StartNow { get; set; }

        [HtmlAttributeName("secondary")]
        public bool Secondary { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";

            output.Attributes.SetAttribute("id", string.IsNullOrEmpty(ButtonId) ? "" : ButtonId);
            output.AddClass("govuk-button");

            if (StartNow)
                output.AddClass("govuk-button--start");
            if (Secondary)
                output.AddClass("govuk-button--secondary");

            output.Attributes.SetAttribute("type", ButtonType.ToString().ToLower());

            if (ButtonStatus == GdsEnums.Status.Disabled)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
                output.Attributes.SetAttribute("aria-disabled", "true");
                output.AddClass("govuk-button--disabled");
            }

            output.Content.SetContent(ButtonText);
        }
    }
}
