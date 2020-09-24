using System.ComponentModel;

namespace GDSHelpers
{
    public partial class GdsEnums
    {
        public enum AdditionalOptions
        {
            [Description("No option")]
            None,
            [Description("true")]
            True,
            [Description("false")]
            False,
        }

        public enum HideBelow
        {
            [Description("")]
            None,

            [Description("hide-tablet-or-below")]
            Tablet,

            [Description("hide-mobile-or-below")]
            Mobile
        }
    }
}
