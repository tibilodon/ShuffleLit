using ShuffleLit.Models;

namespace ShuffleLit.Interfaces
{
    public interface ILiteratureRepository
    {
        Task<IEnumerable<Literature>> GetAll();
        Task<List<Literature>> GetAllUserLiteratures();
        Task<Literature> GetByIdAsync(int id);
        Task<Literature> GetByIdAsyncNoTracking(int id);
        bool Add(Literature literature);
        bool Update(Literature literature);
        bool Delete(Literature literature);
        bool Save();
    }
}
