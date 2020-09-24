using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GDSHelpers
{
    public static class Helpers
    {
        public static string GenerateInfoId(this ModelExpression For)
        {
            return $"{For.Name}-info";
        }

        public static string GenerateHintId(this ModelExpression For)
        {
            return $"{For.Name.ToLower()}-hint";
        }

    }
}
