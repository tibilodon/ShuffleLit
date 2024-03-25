using ShuffleLit.Models;

namespace ShuffleLit.Interfaces
{
    public interface IProfileRepository
    {
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
