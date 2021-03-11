using System.IO;
using System.Text.Encodings.Web;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace GDSHelpers.TagHelpers
{
    [HtmlTargetElement("gds-text-area", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TextAreaHelper : TagHelper
    {
        private readonly decimal _gdsVersion;

        private readonly IHtmlGenerator _htmlGenerator;
        private readonly HtmlEncoder _htmlEncoder;

        public TextAreaHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder, IConfiguration config)
        {
            _gdsVersion = config.GetValue<decimal>("GdsHelpers:GdsToolkitVersion");

            _htmlGenerator = htmlGenerator;
            _htmlEncoder = htmlEncoder;
            CountType = GdsEnums.CountTypes.None;
            UseH2ForLabel = false;
        }

        [HtmlAttributeName("textarea-id")]
        public string TextAreaId { get; set; }

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("count-type")]
        public GdsEnums.CountTypes CountType { get; set; }
        
        [HtmlAttributeName("max-length")]
        public int MaxLength { get; set; }
        
        [HtmlAttributeName("threshold")]
        public int Threshold { get; set; } = 0;

        [HtmlAttributeName("hint-text")]
        public string HintText { get; set; }

        [HtmlAttributeName("use-h2-label")]
        public bool UseH2ForLabel { get; set; }

        [HtmlAttributeName("custom-label")]
        public string CustomLabel { get; set; }

        [HtmlAttributeName("remove-label")]
        public bool RemoveLabel { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("id", string.IsNullOrEmpty(TextAreaId) ? "" : TextAreaId + "div");

            var hasErrors = ViewContext.HasErrorsFor(For.Name);
            var cssClass = hasErrors ? "govuk-form-group govuk-form-group--error" : "govuk-form-group";

            var charCountDataModule = "character-count";
            var charCountClass = "js-character-count";

            var useCounter = CountType != GdsEnums.CountTypes.None;

            if (useCounter)
            {
                output.AddClass("govuk-character-count");

                if (_gdsVersion >= 3m)
                {
                    charCountDataModule = "govuk-character-count";
                    charCountClass = "govuk-js-character-count";
                }

                output.Attributes.Add("data-module", charCountDataModule);

                if (Threshold > 0)
                {
                    output.Attributes.Add("data-threshold", Threshold);
                }

                switch (CountType)
                {
                    case GdsEnums.CountTypes.Characters:
                        output.Attributes.Add("data-maxlength", MaxLength);
                        break;

                    case GdsEnums.CountTypes.Words:
                        output.Attributes.Add("data-maxwords", MaxLength);
                        break;
                }

                var divStart = $"<div class=\"{cssClass}\">";
                output.PreContent.AppendHtml(divStart);

                output.PostContent.AppendHtml("</div>");
            }
            else
            {
                output.Attributes.Add("class", cssClass);
            }

            var modelBuilder = new ModelBuilder
            {
                For = For,
                ViewContext = ViewContext,
                HtmlEncoder = _htmlEncoder,
                HtmlGenerator = _htmlGenerator
            };

            using var writer = new StringWriter();
            if (!RemoveLabel)
            {
                modelBuilder.WriteLabel(writer, "", CustomLabel, UseH2ForLabel);
            }

            if (!string.IsNullOrEmpty(For.Metadata.Description) || !string.IsNullOrEmpty(HintText))
            {
                modelBuilder.WriteHint(writer, HintText);
            }

            if (hasErrors)
            {
                modelBuilder.AddCssClass("govuk-input--error");
            }

            modelBuilder.WriteValidation(writer);

            modelBuilder.WriteTextArea(writer, useCounter, charCountClass);

            if (CountType != GdsEnums.CountTypes.None)
            {
                modelBuilder.WriteCountInfo(writer);
            }

            output.Content.SetHtmlContent(writer.ToString());
        }

    }
}
