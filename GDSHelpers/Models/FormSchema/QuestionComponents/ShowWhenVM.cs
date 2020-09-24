using Newtonsoft.Json;

namespace GDSHelpers.Models.FormSchema
{
    public class ShowWhenVM
    {
        [JsonProperty("question_id")]
        public string QuestionId { get; set; }


        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("hide_parent_question_in_check_your_answers")]
        public bool HideParentQuestionInCheckYourAnswers { get; set; }

    }
}
