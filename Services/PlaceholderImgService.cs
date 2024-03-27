using ShuffleLit.Data.Enum;
using ShuffleLit.Interfaces;

namespace ShuffleLit.Services
{
    public class PlaceholderImgService : IPlaceholderImgService
    {
        public string PlaceholderImg(LiteratureCategory enumId)
        {
            if (enumId == LiteratureCategory.Podcast)
            {
                return "https://www.svgrepo.com/show/533307/podcast.svg";
            }
            return "https://www.svgrepo.com/show/533406/book.svg";
        }
    }
}
