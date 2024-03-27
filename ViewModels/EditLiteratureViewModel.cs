using ShuffleLit.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace ShuffleLit.ViewModels
{
    public class EditLiteratureViewModel
    {
        //  id field must be present, but ought to be optional(nullable)
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public LiteratureCategory LiteratureCategory { get; set; }
        [Required]

        public LiteratureState LiteratureState { get; set; }
        [Required]
        public string LinkUrl { get; set; }
        //public string AppUserId { get; set; }
    }
}
