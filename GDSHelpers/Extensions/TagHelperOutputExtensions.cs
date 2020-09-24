using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.Extensions
{
    public static class TagHelperOutputExtensions
    {
        /// <summary>
        /// Adds the given classValue to the tagHelperOutput's TagHelperOutput.Attributes using HtmlEncoder.Default
        /// </summary>
        public static void AddClass(this TagHelperOutput output, string classValue)
        {
            output.AddClass(classValue, HtmlEncoder.Default);
        }
    }
}