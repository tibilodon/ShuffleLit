using ShuffleLit.Data.Enum;

namespace ShuffleLit.Models
{
    public class LiteratureCollection
    {
        public string? AppUserId { get; set; }
        public int LiteratureId { get; set; }
        public Literature Literature { get; set; }
        public AppUser? AppUser { get; set; }
        public LiteratureState LiteratureState { get; set; }

    }
}
