﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using GDSHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GDSHelpers.TagHelpers
{

    [HtmlTargetElement("gds-select", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SelectHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly HtmlEncoder _htmlEncoder;
        
        public SelectHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder)
        {
            _htmlGenerator = htmlGenerator;
            _htmlEncoder = htmlEncoder;
            OptionLabel = "Please Select and Option";
        }

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("list-items")]
        public List<SelectListItem> ListItems { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("option-label")]
        public string OptionLabel { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("govuk-form-group");

            var modelBuilder = new ModelBuilder
            {
                For = For,
                ViewContext = ViewContext,
                HtmlEncoder = _htmlEncoder,
                HtmlGenerator = _htmlGenerator
            };

            using var writer = new StringWriter();
            modelBuilder.WriteLabel(writer);

            if (!string.IsNullOrEmpty(For.Metadata.Description))
            {
                modelBuilder.WriteHint(writer);
            }

            if (ViewContext.HasErrorsFor(For.Name))
            {
                output.AddClass("govuk-form-group--error");
                modelBuilder.AddCssClass("govuk-input--error");
            }

            modelBuilder.WriteValidation(writer);
            modelBuilder.WriteSelect(writer, ListItems, OptionLabel);

            output.Content.SetHtmlContent(writer.ToString());
        }
        
    }
}
