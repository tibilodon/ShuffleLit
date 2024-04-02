using ShuffleLit.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShuffleLit.Models
{
    public class Literature
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public LiteratureCategory LiteratureCategory { get; set; }
        public LiteratureState LiteratureState { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<LiteratureCollection> LiteratureCollections { get; set;}
    }
}
