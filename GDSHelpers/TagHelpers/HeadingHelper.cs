using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GDSHelpers.Extensions;

namespace GDSHelpers.TagHelpers
{
    [Obsolete("Use h1-h6 tag and add gds attribute")]
    [HtmlTargetElement("gds-heading", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class HeadingHelper : TagHelper
    {
        public HeadingHelper()
        {
            Caption = "";
        }

        [HtmlAttributeName("heading-type")]
        public GdsEnums.Headings HeadingType { get; set; }

        [HtmlAttributeName("text")]
        public string Text { get; set; }

        [HtmlAttributeName("caption")]
        public string Caption { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Tag name
            var tag = HeadingType.ToString().ToLower();
            output.TagName = tag;
            output.TagMode = TagMode.StartTagAndEndTag;

            // Class (Added to any existing classes for tag)
            var cssClass = GdsEnums.GetDescriptionFromEnum(HeadingType);
            output.AddClass(cssClass);

            if (!string.IsNullOrEmpty(Caption))
            {
                var caption = new TagBuilder("span");
                caption.MergeAttribute("class", cssClass.Replace("heading", "caption"));
                caption.InnerHtml.Append(Caption);
                output.PreContent.SetHtmlContent(caption);
            }

            output.Content.SetContent(Text);
        }
    }
}
