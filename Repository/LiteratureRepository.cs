using Microsoft.EntityFrameworkCore;
using ShuffleLit.Data;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;

namespace ShuffleLit.Repository
{
    public class LiteratureRepository : ILiteratureRepository
    {
        //  create the sql --> save changes in DB
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //  bring id DB
        public LiteratureRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public bool Add(Literature literature)
        {
            _context.Add(literature);
            return Save();
        }

        public bool Delete(Literature literature)
        {
            _context.Remove(literature);
            return Save();
        }

        public async Task<IEnumerable<Literature>> GetAll()
        {
            //  include join table
            return await _context.Literatures.Include(l => l.LiteratureCollections).ToListAsync();
        }
        //  get all user records
        public async Task<List<Literature>> GetAllUserLiteratures()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userLiteratures = _context.Literatures.Where(l => l.AppUser.Id == curUser).ToList();
            return userLiteratures;
        }

        public async Task<Literature> GetByIdAsync(int id)
        {
            //  include join table
            return await _context.Literatures.Include(l => l.LiteratureCollections).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Literature> GetByIdAsyncNoTracking(int id)
        {
            //  include join table
            return await _context.Literatures.AsNoTracking().Include(l => l.LiteratureCollections).FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            //  return boolean instead of int
            return saved > 0 ? true : false;
        }

        public bool Update(Literature literature)
        {
            _context.Update(literature);
            return Save();
        }
    }
}
