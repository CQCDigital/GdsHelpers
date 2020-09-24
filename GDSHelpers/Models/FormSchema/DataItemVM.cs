using System.Collections.Generic;
using Newtonsoft.Json;

namespace GDSHelpers.Models.FormSchema
{
    public class DataItemVM
    {
        [JsonProperty("variable_id")]
        public string Id { get; set; }

        [JsonProperty("variable_notes")]
        public string Notes { get; set; }

        [JsonProperty("variable_value")]
        public string Value { get; set; }
    }
}
