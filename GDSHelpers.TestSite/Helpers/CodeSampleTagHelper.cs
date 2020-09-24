using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GDSHelpers.TestSite.Helpers
{
    [HtmlTargetElement("code-sample", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CodeSampleTagHelper : PartialTagHelper
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Show a preview of the generated html code (default = true)
        /// </summary>
        [HtmlAttributeName("show-html")]
        public bool ShowHtml { get; set; } = true;

        /// <summary>
        /// Show a rendered preview (default = true)
        /// </summary>
        [HtmlAttributeName("show-preview")]
        public bool ShowPreview { get; set; } = true;

        /// <summary>
        /// Show the raw razor code (default = true)
        /// </summary>
        [HtmlAttributeName("show-razor")]
        public bool ShowRazor { get; set; } = true;


        public CodeSampleTagHelper(IWebHostEnvironment webHostEnvironment, ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
        {
            _viewEngine = viewEngine;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);
            var content = output.Content.GetContent();

            output.TagName = "div";
            output.AddClass("app-example");
            output.AddClass("test");
            output.TagMode = TagMode.StartTagAndEndTag;

            if (!ShowPreview)
            {
                output.Content.Clear();
            }

            if (ShowRazor)
            {
                AddRazorCode(output);
            }

            if (ShowHtml)
            {
                AddCodeBlock(output, content);
            }
        }

        private void AddRazorCode(TagHelperOutput output)
        {
            string html;
            try
            {
                var view = FindView(Name);
                var viewFileLocation = Path.Combine(_webHostEnvironment.ContentRootPath, view.View.Path.Substring(1));
                html = File.ReadAllText(viewFileLocation) ?? $"Could not get view {Name} - {viewFileLocation}";
            }
            catch (Exception ex)
            {
                html = ex.Message;
            }

            AddCodeBlock(output, html);
        }

        private static void AddCodeBlock(TagHelperOutput output, string html)
        {
            html = WebUtility.HtmlEncode(html);
            html = html?.Replace("\r\n", "<br />").Replace("\n", "<br />");
            output.Content.AppendHtml($"<div class=\"app-example__code\">{html}</div>");
        }

        private ViewEngineResult FindView(string partialName)
        {
            var viewEngineResult = _viewEngine.GetView(ViewContext.ExecutingFilePath, partialName, isMainPage: false);
            var getViewLocations = viewEngineResult.SearchedLocations;
            if (!viewEngineResult.Success)
            {
                viewEngineResult = _viewEngine.FindView(ViewContext, partialName, isMainPage: false);
            }

            if (!viewEngineResult.Success)
            {
                var searchedLocations = Enumerable.Concat(getViewLocations, viewEngineResult.SearchedLocations);
                return ViewEngineResult.NotFound(partialName, searchedLocations);
            }

            return viewEngineResult;
        }

        
    }
}
