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

        public bool DeleteLiteratureFromUser(string appUserId, int literatureId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LiteratureCollection>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LiteratureCollection>> GetCollectionForUser(string appUserId)
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
