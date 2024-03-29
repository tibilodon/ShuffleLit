using ShuffleLit.Models;

namespace ShuffleLit.Interfaces
{
    public interface ILiteratureCollectionRepository
    {
        Task<IEnumerable<LiteratureCollection>> GetAll();
        Task<List<LiteratureCollection>> GetAllLiteratureCollections();
        Task<LiteratureCollection> GetByIdAsync(int id);
        Task<LiteratureCollection> GetByIdAsyncNoTracking(int id);

        bool Add(LiteratureCollection literatureCollection);
        bool Update(LiteratureCollection literatureCollection);
        bool Delete(LiteratureCollection literatureCollection);
        bool Save();
    }
}
