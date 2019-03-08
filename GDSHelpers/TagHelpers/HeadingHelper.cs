﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

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
            var tag = HeadingType.ToString().ToLower();
            var cssClass = GdsEnums.GetCssClassFromEnum(HeadingType);

            output.TagName = tag;
            output.Attributes.SetAttribute("class", cssClass);

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
