using Microsoft.EntityFrameworkCore;
using ShuffleLit.Data;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;

namespace ShuffleLit.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //
        public AppUser GetAppUser(string appUserId)
        {
            return _context.Users.Where(o => o.Id == appUserId).FirstOrDefault();
        }

        public ICollection<AppUser> GetAppUserOfALiterature(int literatureId)
        {
            return _context.LiteratureCollections.Where(lit => lit.Literature.Id == literatureId).Select(u => u.AppUser).ToList();
        }

        public ICollection<AppUser> GetAppUsers()
        {
            return _context.Users.ToList();
        }

        public ICollection<Literature> GetLiteratureByAppUserId(string appUserId)
        {
            return _context.LiteratureCollections.Where(lc => lc.AppUser.Id == appUserId).Select(lit => lit.Literature).ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        //  return bool instead of int
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {

            //  update user obj
            _context.Update(user);
            //  call Save() method
            return Save();
        }
    }
}
