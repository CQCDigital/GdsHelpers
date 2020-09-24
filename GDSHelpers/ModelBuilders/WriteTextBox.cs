using System.Collections;
using System.Collections.Generic;
using System.IO;
using static GDSHelpers.GdsEnums;

namespace GDSHelpers
{
    public partial class ModelBuilder
    {
        public Autocomplete AutoComplete { get; set; }

        public AdditionalOptions Spellcheck { get; set; }

        public string TextBoxId { get; set; }
        public int MaxLength { get; set; }

        public TextTransform TextTransform { get; set; }
        public string TextBoxWidthChars {get; set;}

        public void WriteTextBox(TextWriter writer  )
        {
            var tagBuilder = HtmlGenerator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                For.Model,
                null,
                new { @class = "govuk-input" });
            
            if (TextTransform != TextTransform.None)
            {
                tagBuilder.AddCssClass(GetDescriptionFromEnum(TextTransform));
            }

            if (!string.IsNullOrWhiteSpace(TextBoxWidthChars))
            {
                tagBuilder.AddCssClass($"govuk-input--width-{TextBoxWidthChars}");
            }

            ApplyCss(tagBuilder);

            if (AutoComplete != Autocomplete.Null)
                tagBuilder.MergeAttribute("autocomplete", GetDescriptionFromEnum(this.AutoComplete));
            if (Spellcheck != AdditionalOptions.None)
                tagBuilder.MergeAttribute("spellcheck", GetDescriptionFromEnum(this.Spellcheck));
            if (MaxLength != 0)
                tagBuilder.MergeAttribute("maxlength", this.MaxLength.ToString());

            if (!string.IsNullOrEmpty(LabelId))
                tagBuilder.MergeAttribute("aria-labelledby", LabelId);
            if (!string.IsNullOrEmpty(For.Name))
                tagBuilder.MergeAttribute("aria-describedby", HiddenSpanId ?? LabelId); //if we have hidden span, then there is no label txt

            tagBuilder.WriteTo(writer, HtmlEncoder);
        }
    }
}
