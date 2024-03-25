using ShuffleLit.Data.Enum;

namespace ShuffleLit.ViewModels
{
    public class CreateLiteratureViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public LiteratureCategory LiteratureCategory { get; set; }
        public LiteratureState LiteratureState { get; set; }
        public string LinkUrl { get; set; }
        public string AppUserId { get; set; }
    }
}
