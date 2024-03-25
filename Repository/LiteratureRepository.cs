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
        //  bring id DB
        public LiteratureRepository(ApplicationDbContext context)
        {
            _context = context;
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
            return await _context.Literatures.ToListAsync();
        }

        public async Task<Literature> GetByIdAsync(int id)
        {
            return await _context.Literatures.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Literature> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Literatures.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
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
