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

        public bool AddLiteratureToUser(string appUserId, int literatureId)
        {
            var literatureCollection = new LiteratureCollection
            {
                AppUserId = appUserId,
                LiteratureId = literatureId
            };
            _context.Add(literatureCollection);
            return Save();
        }

        public bool Delete(LiteratureCollection literatureCollection)
        {
            throw new NotImplementedException();
        }

        public async Task<LiteratureCollection> DeleteLiteratureCollectionFromUser(string appUserId, int literatureId)
        {
            var literatureCollection = await _context.LiteratureCollections.FirstOrDefaultAsync(lc => lc.AppUserId == appUserId && lc.LiteratureId == literatureId);

            _context.LiteratureCollections.Remove(literatureCollection);
            return literatureCollection;
            //return Save();

        }

        public async Task<LiteratureCollection> FindAppUserCollectionById(string appUserId, int literatureId)
        {
            return await _context.LiteratureCollections.FirstOrDefaultAsync(lc => lc.AppUserId == appUserId && lc.LiteratureId == literatureId);

        }

        public Task<IEnumerable<LiteratureCollection>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LiteratureCollection>> GetLiteratureCollectionForUser(string appUserId)
        {
            throw new NotImplementedException();
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
