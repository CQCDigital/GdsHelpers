using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-column", ParentTag = "gds-row")]
    public class ColumnHelper : TagHelper
    {
        public ColumnHelper()
        {
            HideBelow = GdsEnums.HideBelow.None;
        }

        [HtmlAttributeName("desktop-size")]
        public GdsEnums.DesktopColumns DesktopSize { get; set; }

        [HtmlAttributeName("tablet-size")]
        public GdsEnums.TabletColumns TabletSize { get; set; }

        [HtmlAttributeName("hide-at-or-below-size")]
        public GdsEnums.HideBelow HideBelow { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tabletClass = GdsEnums.GetDescriptionFromEnum(TabletSize);
            var desktopClass = GdsEnums.GetDescriptionFromEnum(DesktopSize);
            var hideBelow = GdsEnums.GetDescriptionFromEnum(HideBelow);

            output.TagName = "div";
            output.AddClass(tabletClass);
            output.AddClass(desktopClass);
            output.AddClass(hideBelow);

            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
