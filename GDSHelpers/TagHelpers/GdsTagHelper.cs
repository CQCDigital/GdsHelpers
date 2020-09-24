using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    public abstract class GdsTagHelper : TagHelper
    {
        private protected GdsHelperSettings Settings;

        protected GdsTagHelper(IConfiguration config)
        {
            Settings = GdsHelperSettings.CreateSingleton(config);
        }

        /// <summary>
        /// Style this HTML element with standard GDS styles
        /// </summary>
        [HtmlAttributeName("gds")]
        public bool Gds { get; set; }
    }
}