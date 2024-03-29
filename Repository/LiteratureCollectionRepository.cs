using Microsoft.EntityFrameworkCore;
using ShuffleLit.Data;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;

namespace ShuffleLit.Repository
{
    public class LiteratureCollectionRepository : ILiteratureCollectionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LiteratureCollectionRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;


        }
        public bool Add(LiteratureCollection literatureCollection)
        {
            _context.Add(literatureCollection);
            return Save();
        }

        public bool Delete(LiteratureCollection literatureCollection)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LiteratureCollection>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<LiteratureCollection>> GetAllLiteratureCollections()
        {
            throw new NotImplementedException();
        }

        public Task<LiteratureCollection> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<LiteratureCollection> GetByIdAsyncNoTracking(int id)
        {
            return await _context.LiteratureCollections.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(LiteratureCollection literatureCollection)
        {
            _context.Update(literatureCollection);
            return Save();
        }
    }
}
