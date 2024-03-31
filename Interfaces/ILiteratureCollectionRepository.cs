using ShuffleLit.Models;

namespace ShuffleLit.Interfaces
{
    public interface ILiteratureCollectionRepository
    {
        Task<IEnumerable<LiteratureCollection>> GetAll();
        //Task<List<LiteratureCollection>> GetAllLiteratureCollections();
        //Task<LiteratureCollection> GetByIdAsync(int id);
        //Task<LiteratureCollection> GetByIdAsyncNoTracking(int id);
        bool Add(LiteratureCollection literatureCollection);
        bool AddLiteratureToUser(string appUserId, int literatureId);
        //bool Update(LiteratureCollection literatureCollection);
        bool Delete(LiteratureCollection literatureCollection);
        Task<LiteratureCollection> DeleteLiteratureCollectionFromUser(string appUserId, int literatureId);
        bool Save();
        Task<IEnumerable<LiteratureCollection>> GetLiteratureCollectionForUser(string appUserId);
        Task<LiteratureCollection> FindAppUserCollectionById(string appUserId, int literatureId);
    }
}
