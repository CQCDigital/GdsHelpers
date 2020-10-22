using System.Linq;
using System.Text;
using GDSHelpers.Models.FormSchema;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GDSHelpers
{
    public static class InlineHelpers
    {
        private static string[] _defaultContentOrder = { "instruction_text", "question", "additional_text", "html_content", "noscript" };

        /// <summary>
        /// Creates a GDS compliant Question with label, help text and error messages
        /// </summary>
        /// <param name="helper">The HTML Class</param>
        /// <param name="question">The QuestionVM class</param>
        /// <param name="htmlAttributes">Any additional attributes to apply to the questions.</param>
        /// <returns>Returns the HTML for GDS compliant questions</returns>
        public static IHtmlContent GdsQuestion(this IHtmlHelper helper, QuestionVM question, double gdsVersion = 0, object htmlAttributes = null)
        {
            IHtmlContent content;

            switch (question.InputType)
            {
                case "textbox":
                    content = BuildTextBox(question);
                    break;

                case "textarea":
                    content = BuildTextArea(question, gdsVersion);
                    break;

                case "optionlist":
                    content = BuildOptionList(question);
                    break;

                case "optionlist_small":
                    content = BuildOptionList(question, "small");
                    break;

                case "selectlist":
                    content = BuildSelectList(question);
                    break;

                case "checkboxlist":
                    content = BuildCheckboxList(question);
                    break;

                case "conditionaltext":
                    content = BuildConditionalText(question);
                    break;

                default:
                    content = BuildInfoPage(question);
                    break;

            }

            return new HtmlString(content.ToString());

        }

        private static IHtmlContent BuildInfoPage(QuestionVM question)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(question.Question))
                sb.AppendLine($"<p class=\"govuk-body gds-question\">{question.Question}</p>");

            if (!string.IsNullOrEmpty(question.AdditionalText))
                sb.AppendLine($"<p class=\"govuk-body gds-hint\">{question.AdditionalText}</p>");

            if (!string.IsNullOrEmpty(question.HtmlContent))
                sb.AppendLine(question.HtmlContent);

            return new HtmlString(sb.ToString());

        }

        private static IHtmlContent BuildConditionalText(QuestionVM question)
        {
            var elementId = question.QuestionId;

            var showWhen = CreateShowWhenAttributes(question);
            var showWhenCss = string.IsNullOrEmpty(showWhen) ? "" : "gds-display-none";

            var questionId = $"id=\"q{elementId}\"";

            var sb = new StringBuilder();

            sb.AppendLine($"<div {questionId} class=\"govuk-form-group {showWhenCss}\" {showWhen}>");

            if (!string.IsNullOrEmpty(question.InstructionText))
                sb.AppendLine($"<p class=\"govuk-body\">{question.InstructionText}</p>");

            sb.AppendLine($"<label class=\"govuk-label gds-question\" for=\"{elementId}\">{question.Question}</label>");

            if (!string.IsNullOrEmpty(question.AdditionalText))
                sb.AppendLine($"<span id=\"{elementId}-hint\" class=\"govuk-hint gds-hint\">{question.AdditionalText}</span>");

            if (!string.IsNullOrEmpty(question.HtmlContent))
                sb.AppendLine(question.HtmlContent);

            sb.AppendLine("</div>");

            return new HtmlString(sb.ToString());
        }

        private static IHtmlContent BuildTextBox(QuestionVM question)
        {
            var elementId = question.QuestionId;
            var isErrored = question.Validation?.IsErrored == true;
            var errorId = isErrored ? $"{elementId}-error" : "";
            var errorMsg = question.Validation?.ErrorMessage;
            var requiredIf = question.Validation?.RequiredIf?.ErrorMessage == errorMsg;//flag if its a conditional required field
            var erroredCss = isErrored ? "govuk-form-group--error" : "";
            var erroredInputCss = isErrored ? "govuk-input--error" : "";

            var showWhen = CreateShowWhenAttributes(question);
            var showWhenCss = string.IsNullOrEmpty(showWhen) ? "" : "gds-display-none";

            var ariaDescribedBy = $"aria-describedby=\"{elementId}-hint {errorId}\"";
            if (string.IsNullOrEmpty(question.AdditionalText) && !isErrored) ariaDescribedBy = "";

            var questionId = $"id=\"q{elementId}\"";

            var sb = new StringBuilder();

            sb.AppendLine($"<div {questionId} class=\"govuk-form-group {erroredCss} {showWhenCss}\" {showWhen}>");
            sb.AppendLine($"<fieldset class=\"govuk-fieldset\" {ariaDescribedBy}>");

            if (isErrored && requiredIf)//places the error at the top of a group of required fields
                sb.AppendLine($"<span id=\"{errorId}\" class=\"govuk-error-message\">{errorMsg}</span>");


            // Adds our content in the correct order
            sb.GenerateContentOrder(elementId, question);


            if (isErrored && (!requiredIf))//places the error at next to errored fields
                sb.AppendLine($"<span id=\"{errorId}\" class=\"govuk-error-message\">{errorMsg}</span>");

            sb.AppendLine($"<input class=\"govuk-input {erroredInputCss} {question.InputCss}\" id=\"{elementId}\" name=\"{elementId}\" type=\"{question.DataType}\" {ariaDescribedBy} value=\"{question.Answer}\">");

            sb.AppendLine("</fieldset>");
            sb.AppendLine("</div>");

            return new HtmlString(sb.ToString());
        }

        private static IHtmlContent BuildTextArea(QuestionVM question, double gdsVersion)
        {
            var elementId = question.QuestionId;
            var showCounter = question.Validation?.MaxLength != null;
            var counterCount = question.Validation?.MaxLength?.Max;
            var counterType = question.Validation?.MaxLength?.Type == "words" ? "maxwords" : "maxlength";
            var counterThreshold = question.Validation?.MaxLength?.Threshold > 0
                                    ? $"data-threshold=\"{question.Validation?.MaxLength?.Threshold}\""
                                    : "";
            var isErrored = question.Validation?.IsErrored == true;
            var errorId = isErrored ? $"{elementId}-error" : "";
            var errorMsg = question.Validation?.ErrorMessage;
            var erroredCss = isErrored ? "govuk-form-group--error" : "";
            var erroredInputCss = isErrored ? "govuk-textarea--error" : "";
            var inputHeight = string.IsNullOrEmpty(question.InputHeight) ? "5" : question.InputHeight;

            var questionId = $"id=\"q{elementId}\"";

            var showWhen = CreateShowWhenAttributes(question);
            var showWhenCss = string.IsNullOrEmpty(showWhen) ? "" : "gds-display-none";

            var ariaDescribedBy = $"aria-describedby=\"{elementId}-hint {errorId}\"";
            if (string.IsNullOrEmpty(question.AdditionalText) && isErrored == false) ariaDescribedBy = "";


            var charCountDataModule = "character-count";
            var charCountClass = showCounter ? "js-character-count" : "";

            if (gdsVersion >= 3)
            {
                charCountDataModule = "govuk-character-count";
                charCountClass = showCounter ? "govuk-js-character-count" : "";
            }

            var sb = new StringBuilder();

            if (showCounter)
            {
                sb.AppendLine($"<div {questionId} class=\"govuk-character-count {showWhenCss}\" " +
                              $"data-module=\"{charCountDataModule}\" data-{counterType}=\"{counterCount}\" " +
                              $"{counterThreshold} {showWhen}>");
                showWhen = "";
                questionId = "";
                showWhenCss = "";
            }


            sb.AppendLine($"<div {questionId} class=\"govuk-form-group {erroredCss} {showWhenCss}\" {showWhen}>");
            sb.AppendLine($"<fieldset class=\"govuk-fieldset\" {ariaDescribedBy}>");


            // Adds our content in the correct order
            sb.GenerateContentOrder(elementId, question);


            if (isErrored)
                sb.AppendLine($"<span id=\"{errorId}\" class=\"govuk-error-message\">{errorMsg}</span>");

            sb.AppendLine($"<textarea class=\"govuk-textarea {erroredInputCss} {charCountClass} {question.InputCss}\" id=\"{elementId}\" " +
                      $"name=\"{elementId}\" rows=\"{inputHeight}\" {ariaDescribedBy}>{question.Answer}</textarea>");

            sb.AppendLine("</fieldset>");
            sb.AppendLine("</div>");

            if (showCounter)
            {
                sb.AppendLine($"<span id=\"{elementId}-info\" class=\"govuk-hint govuk-character-count__message\" aria-live=\"polite\"></span>");
                sb.AppendLine("</div>");
            }

            return new HtmlString(sb.ToString());
        }

        private static IHtmlContent BuildOptionList(QuestionVM question, string radioSize = null)
        {
            var elementId = question.QuestionId;
            var isErrored = question.Validation?.IsErrored == true;
            var errorId = isErrored ? $"{elementId}-error" : "";
            var errorMsg = question.Validation?.ErrorMessage;
            var erroredCss = isErrored ? "govuk-form-group--error" : "";
            var questionId = $"id=\"q{elementId}\"";
            var lblId = $"q{elementId}-label";
            var inlineCSS = question.ListDirection == "horizontal" ? "govuk-radios--inline" : "";

            var ariaDescribedBy = $"aria-describedby=\"{elementId}-hint {errorId}\"";
            if (string.IsNullOrEmpty(question.Question) && isErrored == false) ariaDescribedBy = "";

            var sb = new StringBuilder();

            var showWhen = CreateShowWhenAttributes(question);
            var showWhenCss = string.IsNullOrEmpty(showWhen) ? "" : "gds-display-none";

            sb.AppendLine($"<div {questionId} class=\"govuk-form-group {erroredCss} {showWhenCss}\" {showWhen}>");
            sb.AppendLine($"<fieldset class=\"govuk-fieldset\" {ariaDescribedBy}>");

            if (!string.IsNullOrEmpty(question.Question))
            {
                sb.AppendLine("<span class=\"govuk-fieldset__legend govuk-fieldset__legend--xl\">");
                sb.AppendLine($"<label id=\"{lblId}\" class=\"govuk-label gds-question\">{question.Question}</label>");
                sb.AppendLine("</span>");

                sb.AppendLine("<legend class=\"govuk-visually-hidden\">");
                sb.AppendLine($"<span>{question.Question}</span>");
                sb.AppendLine("</legend>");
            }

            if (!string.IsNullOrEmpty(question.AdditionalText))
                sb.AppendLine($"<span id=\"{elementId}-hint\" class=\"govuk-hint gds-hint\">{question.AdditionalText}</span>");

            if (!string.IsNullOrEmpty(question.HtmlContent))
                sb.AppendLine(question.HtmlContent);

            if (isErrored)
                sb.AppendLine($"<span id=\"{errorId}\" class=\"govuk-error-message\">{errorMsg}</span>");


            var count = 0;
            var smallRadioCss = (radioSize == "small") ? "govuk-radios--small" : "";
            sb.AppendLine($"<div class=\"govuk-radios {smallRadioCss} {inlineCSS}\">");

            foreach (var option in question.Options)
            {
                var value = option.Value;
                var text = option.Text;
                var hint = option.Hint;

                var checkedCss = question.Answer == value ? "checked" : "";

                sb.AppendLine("<div class=\"govuk-radios__item\">");
                sb.AppendLine($"<input class=\"govuk-radios__input\" id=\"{elementId}-{count}\" name=\"{elementId}\" type=\"radio\" value=\"{value}\" {checkedCss}>");
                sb.AppendLine($"<label class=\"govuk-label govuk-radios__label\" for=\"{elementId}-{count}\">{text}</label>");

                if (!string.IsNullOrEmpty(hint))
                    sb.AppendLine($"<span id=\"{elementId}-{count}-item-hint\" class=\"govuk-hint govuk-radios__hint\">{hint}</span>");

                sb.AppendLine("</div>");
                count += 1;

            }

            sb.AppendLine("</div>");

            sb.AppendLine("</fieldset>");
            sb.AppendLine("</div>");

            return new HtmlString(sb.ToString());
        }

        private static IHtmlContent BuildSelectList(QuestionVM question)
        {
            var elementId = question.QuestionId;
            var isErrored = question.Validation?.IsErrored == true;
            var errorId = isErrored ? $"{elementId}-error" : "";
            var errorMsg = question.Validation?.ErrorMessage;
            var erroredCss = isErrored ? "govuk-form-group--error" : "";
            var questionId = $"id=\"q{elementId}\"";

            var sb = new StringBuilder();

            var showWhen = CreateShowWhenAttributes(question);
            var showWhenCss = string.IsNullOrEmpty(showWhen) ? "" : "gds-display-none";

            sb.AppendLine($"<div {questionId} class=\"govuk-form-group {erroredCss} {showWhenCss}\" {showWhen}>");


            // Adds our content in the correct order
            sb.GenerateContentOrder(elementId, question);


            if (isErrored)
                sb.AppendLine($"<span id=\"{errorId}\" class=\"govuk-error-message\">{errorMsg}</span>");

            sb.AppendLine($"<select class=\"govuk-select\" id=\"{elementId}\" name=\"{elementId}\">");
            sb.AppendLine("<option value>Please select</option>");
            if (question.Options != null)
            {
                foreach (var option in question.Options)
                {
                    var isSelected = question.Answer == option.Value ? "checked" : "";
                    sb.AppendLine($"<option value=\"{option.Value}\" {isSelected}>{option.Text}</option>");
                }
            }
            sb.AppendLine("</select>");
            sb.AppendLine("</div>");


            return new HtmlString(sb.ToString());

        }

        private static IHtmlContent BuildCheckboxList(QuestionVM question)
        {
            var elementId = question.QuestionId;
            var isErrored = question.Validation?.IsErrored == true;
            var errorId = isErrored ? $"{elementId}-error" : "";
            var errorMsg = question.Validation?.ErrorMessage;
            var erroredCss = isErrored ? "govuk-form-group--error" : "";
            var questionId = $"id=\"q{elementId}\"";
            var lblId = $"q{elementId}-label";

            var ariaDescribedBy = $"aria-describedby=\"{lblId} {errorId}\"";
            if (string.IsNullOrEmpty(question.Question) && isErrored == false) ariaDescribedBy = "";

            var sb = new StringBuilder();

            var showWhen = CreateShowWhenAttributes(question);
            var showWhenCss = string.IsNullOrEmpty(showWhen) ? "" : "gds-display-none";

            sb.AppendLine($"<div {questionId} class=\"govuk-form-group {erroredCss} {showWhenCss}\" {showWhen}>");
            sb.AppendLine($"<fieldset class=\"govuk-fieldset\" {ariaDescribedBy}>");

            if (!string.IsNullOrEmpty(question.Question))
            {
                sb.AppendLine("<span class=\"govuk-fieldset__legend govuk-fieldset__legend--xl\">");
                sb.AppendLine($"<label id=\"{lblId}\" class=\"govuk-label gds-question\">{question.Question}</label>");
                sb.AppendLine("</span>");
            }

            if (!string.IsNullOrEmpty(question.AdditionalText))
                sb.AppendLine($"<span id=\"{elementId}-hint\" class=\"govuk-hint gds-hint\">{question.AdditionalText}</span>");

            if (!string.IsNullOrEmpty(question.HtmlContent))
                sb.AppendLine(question.HtmlContent);

            if (isErrored)
                sb.AppendLine($"<span id=\"{errorId}\" class=\"govuk-error-message\">{errorMsg}</span>");


            sb.AppendLine($"<div class=\"govuk-checkboxes\">");
            if (question.Options != null)
            {
                var count = 0;
                foreach (var option in question.Options)
                {
                    var value = option.Value;
                    var text = option.Text;
                    var hint = option.Hint;

                    var checkedCss = question.Answer == value ? "checked" : "";

                    sb.AppendLine("<div class=\"govuk-checkboxes__item\">");
                    sb.AppendLine($"<input class=\"govuk-checkboxes__input\" id=\"{elementId}-{count}\" name=\"{elementId}\" type=\"checkbox\" value=\"{value}\" {checkedCss}>");
                    sb.AppendLine($"<label class=\"govuk-label govuk-checkboxes__label\" for=\"{elementId}-{count}\">{text}</label>");

                    if (!string.IsNullOrEmpty(hint))
                        sb.AppendLine($"<span id=\"{elementId}-{count}-item-hint\" class=\"govuk-hint govuk-checkboxes__hint\">{hint}</span>");

                    sb.AppendLine("</div>");
                    count += 1;
                }
            }
            sb.AppendLine("</div>");


            sb.AppendLine("</fieldset>");
            sb.AppendLine("</div>");

            return new HtmlString(sb.ToString());

        }

        public static IHtmlContent GdsButton(this IHtmlHelper helper, string buttonType, string buttonText, object htmlAttributes = null)
        {
            var button = new TagBuilder("button");
            button.Attributes.Add("class", "govuk-button");
            button.Attributes.Add("type", buttonType.ToLower());
            button.InnerHtml.Append(buttonText);

            button.MergeHtmlAttributes(htmlAttributes);

            return button;
        }


        public static IHtmlContent RenderPreAmble(this IHtmlHelper helper, PageVM page)
        {
            IHtmlContent content = new HtmlString(page.PreAmble);
            return new HtmlString(content.ToString());
        }

        public static IHtmlContent RenderPostAmble(this IHtmlHelper helper, PageVM page)
        {
            IHtmlContent content = new HtmlString(page.PostAmble);
            return new HtmlString(content.ToString());
        }



        private static StringBuilder GenerateContentOrder(this StringBuilder sb, string elementId, QuestionVM question)
        {
            // Get the default content order
            var contentOrder = _defaultContentOrder;

            // Update our order if we have a different order to the default
            if (question.ContentOrder != null && question.ContentOrder.Any())
                contentOrder = question.ContentOrder;


            // Append any missing values to the end of the contentOrder Array
            var exceptions = _defaultContentOrder.Except(contentOrder);
            contentOrder = contentOrder.Concat(exceptions).ToArray();


            // Loop through the order and get our content
            foreach (var item in contentOrder)
            {
                switch (item)
                {
                    case "instruction_text":
                        if (!string.IsNullOrEmpty(question.InstructionText))
                            sb.AppendLine($"<p class=\"govuk-body\">{question.InstructionText}</p>");
                        break;

                    case "question":
                        if (!string.IsNullOrEmpty(question.Question))
                        {
                            sb.AppendLine($"<label class=\"govuk-label gds-question\" for=\"{elementId}\">{question.Question}</label>");

                            sb.AppendLine("<legend class=\"govuk-visually-hidden\">");
                            sb.AppendLine($"<span>{question.Question}</span>");
                            sb.AppendLine("</legend>");
                        }
                        break;

                    case "additional_text":
                        if (!string.IsNullOrEmpty(question.AdditionalText))
                            sb.AppendLine($"<span id=\"{elementId}-hint\" class=\"govuk-hint gds-hint\">{question.AdditionalText}</span>");
                        break;

                    case "html_content":
                        if (!string.IsNullOrEmpty(question.HtmlContent))
                            sb.AppendLine(question.HtmlContent);
                        break;

                    case "noscript":
                        if (!string.IsNullOrEmpty(question.NoScript))
                            sb.AppendLine($"<noscript>{question.NoScript}</noscript>");
                        break;
                }
            }
            return sb;
        }

        private static void MergeHtmlAttributes(this TagBuilder tagBuilder, object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                foreach (var customAttribute in customAttributes)
                {
                    tagBuilder.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                }
            }
        }

        private static string CreateShowWhenAttributes(QuestionVM question)
        {
            if (question.ShowWhen == null
                || string.IsNullOrEmpty(question.ShowWhen.QuestionId)
                || string.IsNullOrEmpty(question.ShowWhen.Answer))
                return "";

            return $" data-showwhen-questionid=\"{question.ShowWhen.QuestionId}\" data-showwhen-value=\"{question.ShowWhen.Answer}\" ";
        }

    }
}
