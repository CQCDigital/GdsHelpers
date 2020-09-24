using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;

namespace GDSHelpers
{
    public partial class ModelBuilder
    {
        public IHtmlGenerator HtmlGenerator { get; set; }
        public HtmlEncoder HtmlEncoder { get; set; }
        public ModelExpression For { get; set; }
        public ViewContext ViewContext { get; set; }

        public string LabelId { get; set; }
        public string HiddenSpanId { get; set; }

        public void AddCssClass(string cssClass)
        {
            _cssClasses.Add(cssClass);
        }
        private readonly IList<string> _cssClasses = new List<string>();

        /// <summary>
        /// Will be true if view declares a base class, but you are passing a derived class
        /// </summary>
        public bool IsViewModelTypeDifferentThanRuntimeType
        {
            get
            {
                var viewModelType = For.Metadata.ContainerType.Name;
                var runtimeModelType = For.ModelExplorer.Container.ModelType.Name;
                bool result = (viewModelType != runtimeModelType);
                return result;
            }
        }

        #region Label

        /// <summary>
        /// Original code that is generating the label
        /// </summary>
        protected void GenerateLabelHtml(TextWriter writer, string hiddenSpan = "", string customLabel = "", bool useH2ForLabel = false)
        {
            LabelId = $"lbl{For.Name}";

            //Get the Labels TEXT, fallback the the fields NAME if no DisplayName or CustomLabel used
            var lblText = string.IsNullOrEmpty(customLabel) ? For.Metadata.DisplayName ?? For.Name : customLabel;

            //Create H2 tag, this sits within the <label> element if used
            var h2 = useH2ForLabel ? $"<h2 class=\"govuk-heading-m\">{lblText}</h2>" : lblText;

            //Build the Label
            var label = new TagBuilder("label");
            label.MergeAttribute("id", LabelId);
            label.MergeAttribute("class", "govuk-label");
            label.MergeAttribute("for", For.Name);
            label.InnerHtml.AppendHtml(h2);

            //Insert the span into the label if needed
            if (!string.IsNullOrWhiteSpace(hiddenSpan) && hiddenSpan.Length > 1)
            {
                HiddenSpanId = $"hidSpan{For.Name}";
                //Build a visually hidden <span>
                var span = new TagBuilder("span");
                span.MergeAttribute("id", HiddenSpanId);
                span.MergeAttribute("class", "govuk-visually-hidden");
                span.InnerHtml.AppendHtml(hiddenSpan);

                label.InnerHtml.AppendHtml(span);
            }

            //Return the label
            label.WriteTo(writer, HtmlEncoder);
        }

        /// <summary>
        /// Updated version of generating label, it's checking if runtime model type is = view model type
        /// if not it tries to get displayName attribute from runtime
        /// </summary>
        public void WriteLabel(TextWriter writer, string hiddenSpan = "", string customLabel = "", bool useH2ForLabel = false)
        {
            //check if model declared in View is different than actual model
            if (string.IsNullOrEmpty(customLabel) 
                && IsViewModelTypeDifferentThanRuntimeType)
            {
                //using reflection we get displayName attr from runtime Model
                customLabel = GetDisplayNameAttributeFromProperty(For.Name);
            }

            GenerateLabelHtml(writer, hiddenSpan, customLabel, useH2ForLabel);
        }

        /// <summary>
        /// Tries to get DisplayNameAttribute or DisplayAttribute from runtime model property
        /// </summary>
        private string GetDisplayNameAttributeFromProperty(string propertyName)
        {
            //it's an actual runtime model, not viewModel declared on view
            MemberInfo property = For.ModelExplorer.Container.ModelType.GetProperty(propertyName);
            return property?.GetCustomAttribute(typeof(DisplayNameAttribute)) is DisplayNameAttribute dd
                ? dd.DisplayName
                : property?.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute da
                    ? da.Name
                    : null;
        }

        #endregion

        #region WriteHint
        public void WriteHint(TextWriter writer, string description ="")
        {
            var lbl = new TagBuilder("span");
            lbl.MergeAttribute("id", For.GenerateHintId());
            lbl.MergeAttribute("class", "govuk-hint");
            lbl.InnerHtml.Append(For.Metadata.Description ?? description);
            lbl.WriteTo(writer, HtmlEncoder);
        }
        public void WriteValidation(TextWriter writer)
        {
            var tagBuilder = HtmlGenerator.GenerateValidationMessage(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                message: null,
                tag: null,
                htmlAttributes: new { @class = "govuk-error-message" });

            tagBuilder.WriteTo(writer, HtmlEncoder);
        }
        public void WriteCountInfo(TextWriter writer)
        {
            var lbl = new TagBuilder("span");
            lbl.MergeAttribute("id", For.GenerateInfoId());
            lbl.MergeAttribute("class", "govuk-hint govuk-character-count__message");
            lbl.MergeAttribute("aria-live", "polite");
            lbl.InnerHtml.Append("");
            lbl.WriteTo(writer, HtmlEncoder);
        }

        #endregion

        #region WriteTextArea
        public void WriteTextArea(TextWriter writer, bool addCounter = false, string charCountClass = "")
        {
            var tagBuilder = HtmlGenerator.GenerateTextArea(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                5,
                80,
                new { @class = "govuk-textarea" });

            if (addCounter)
            {
                tagBuilder.AddCssClass(charCountClass);
            }

            ApplyCss(tagBuilder);

            tagBuilder.Attributes.Remove("maxlength");

            if (!string.IsNullOrEmpty(For.Metadata.Description))
                tagBuilder.MergeAttribute("aria-describedby", For.GenerateHintId());

            tagBuilder.WriteTo(writer, HtmlEncoder);
        }

        #endregion

        #region WriteSelect
        public void WriteSelect(TextWriter writer, List<SelectListItem> listItems, string optionLabel)
        {
            var tagBuilder = HtmlGenerator.GenerateSelect(
                ViewContext,
                For.ModelExplorer,
                optionLabel,
                For.Name,
                listItems,
                false,
                new { @class = "govuk-select" });

            ApplyCss(tagBuilder);

            if (!string.IsNullOrEmpty(For.Metadata.Description))
                tagBuilder.MergeAttribute("aria-describedby", For.GenerateHintId());

            tagBuilder.WriteTo(writer, HtmlEncoder);
        }
        #endregion

        /// <summary>
        /// Adds the CSS classes to the tag being built by the tagBuilder
        /// </summary>
        private void ApplyCss(TagBuilder tagBuilder)
        {
            foreach (var cssClass in _cssClasses)
            {
                tagBuilder.AddCssClass(cssClass);
            }
        }
    }
}
