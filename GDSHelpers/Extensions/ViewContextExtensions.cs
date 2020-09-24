using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GDSHelpers.Extensions
{
    internal static class ViewContextExtensions
    {
        internal static bool HasErrorsFor(this ViewContext context, string fieldName)
        {
            context.ViewData.ModelState.TryGetValue(fieldName, out var entry);
            return entry?.Errors?.Any() == true;
        }
    }
}