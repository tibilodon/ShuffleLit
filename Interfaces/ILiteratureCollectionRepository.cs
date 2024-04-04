using ShuffleLit.Models;

namespace ShuffleLit.Interfaces
{
    public interface ILiteratureCollectionRepository
    {
        bool Add(LiteratureCollection literatureCollection);
        bool AddLiteratureToUser(string appUserId, int literatureId);
        //bool Update(LiteratureCollection literatureCollection);
        Task<LiteratureCollection> DeleteLiteratureCollectionFromUser(string appUserId, int literatureId);
        bool Save();
        Task<List<Literature>> GetLiteratureCollectionForUser();
        Task<LiteratureCollection> FindAppUserCollectionById(string appUserId, int literatureId);
        bool Update(LiteratureCollection literatureCollection);
    }
}
