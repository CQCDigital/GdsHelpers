using GDSHelpers.Enums;

namespace GDSHelpers.Extensions
{
    internal static class GdsSizeExtensions
    {
        public static string ToClassString(this GdsSize size)
        {
            switch (size)
            {
                case GdsSize.ExtraLarge: return "xl";
                case GdsSize.Large: return "l";
                case GdsSize.Medium: return "m";
                case GdsSize.Small: return "s";
                default: return null;
            }
        }
    }
}