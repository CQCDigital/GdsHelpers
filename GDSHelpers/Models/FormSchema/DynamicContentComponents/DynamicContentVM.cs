using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GDSHelpers.Models.FormSchema
{
    public class DynamicContentVM
    {
        [JsonProperty("conditions")]
        public IEnumerable<ConditionVM> Conditions { get; set; }


        [JsonProperty("default-action")]
        public string DefaultAction { get; set; }

    }

    public class ConditionVM
    {
        [JsonProperty("dev_notes")]
        public string DevNotes { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("logic")]
        public LogicVM Logic { get; set; }

        [JsonProperty("if_true")]
        public IfTrueVM IfTrue { get; set; }

    }

    public class LogicVM
    {
        [JsonProperty("variable_source")]
        public string Source { get; set; }

        [JsonProperty("variable_name")]
        public string Variable { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("variable_answer")]
        public string Answer { get; set; }
    }

    public class IfTrueVM
    {
        [JsonProperty("override_question")]
        public QuestionVM OverrideQuestion { get; set; }

        [JsonProperty("override_page")]
        public PageVM OverridePage { get; set; }

        [JsonProperty("do_other")]
        public string OtherAction { get; set; }

        [JsonProperty("conditions")]
        public IEnumerable<ConditionVM> Conditions { get; set; }
    }

}
