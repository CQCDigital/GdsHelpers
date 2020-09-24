using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GDSHelpers.TestSite.Models
{
    public class SampleModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Interesting fact")]
        public string InterestingFact { get; set; }

        [DisplayName(" ")]
        public string EmptyDisplayName { get; set; }
    }

    public class ChildOfSampleModel : SampleModel
    {
        [DisplayName("This is even more interesting fact")]
        public new string InterestingFact { get; set; }
    }
}
