using System.Collections.Generic;
using Newtonsoft.Json;

namespace GDSHelpers.Models.FormSchema
{
    public class PageVM
    {
        [JsonProperty("page_id")]
        public string PageId { get; set; }


        [JsonProperty("page_name")]
        public string PageName { get; set; }
        

        [JsonProperty("dynamic_content")]
        public DynamicContentVM DynamicContent { get; set; }


        [JsonProperty("pre_amble")]
        public string PreAmble { get; set; }


        [JsonProperty("questions")]
        public IEnumerable<QuestionVM> Questions { get; set; }

        [JsonProperty("post_amble")]
        public string PostAmble { get; set; }

        [JsonProperty("buttons")]
        public IEnumerable<ButtonVM> Buttons { get; set; }

        [JsonProperty("next_page_id")]
        public string NextPageId { get; set; }

        [JsonProperty("next_page_reference_id")]//page to reference for next page id
        public string NextPageReferenceId { get; set; }

        [JsonProperty("previous_pages")]
        public IEnumerable<PreviousPageVM> PreviousPages { get; set; }

        [JsonProperty("previous_page_id")]
        public string PreviousPageId { get; set; }

        [JsonProperty("page_title")]
        public string PageTitle { get; set; }

        [JsonProperty("show_related_content")]
        public bool ShowRelatedContent { get; set; }

        [JsonProperty("change_mode_trigger_page_id")]
        public string ChangeModeTriggerPageId { get; set; }

        [JsonProperty("path_change_question")]
        public PathChangePageVM PathChangeQuestion { get; set; }

        [JsonProperty("display_order")]
        public int DisplayOrder { get; set; }

        [JsonProperty("remove_from_submission")]
        public bool RemoveFromSubmission { get; set; }

    }
}
