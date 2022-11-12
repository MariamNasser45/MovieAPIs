
//Implementation of interface "body of this method is identical code of genres controller

using MovieAPIs.Models;

namespace MovieAPIs.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;

        }

        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAllGenre()
        {
            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();//using OrderBy : postman by default arrange genres by ID but the arrange them by alphabatic is the better 
            return genres;
        }
        public async Task <Genre> GetById(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

        }

        public Task<bool> IsvallideGenreid(byte id)
        {
           return _context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }

     
    }
}
