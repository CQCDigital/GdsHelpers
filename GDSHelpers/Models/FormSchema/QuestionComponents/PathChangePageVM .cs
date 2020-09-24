using Newtonsoft.Json;

namespace GDSHelpers.Models.FormSchema
{
    public class PathChangePageVM
    {

        [JsonProperty("question_id")]
        public string QuestionId { get; set; }


        [JsonProperty("answer")]
        public string Answer { get; set; }


        [JsonProperty("next_page_id")]
        public string NextPageId { get; set; }

    }
}
