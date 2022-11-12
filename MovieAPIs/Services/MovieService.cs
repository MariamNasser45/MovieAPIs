using Microsoft.EntityFrameworkCore;

namespace MovieAPIs.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Movie> Add(Movie movie)
        {
            _context.AddAsync(movie);
            _context.SaveChangesAsync();
            return movie;   
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAllMovie()
        {
            return await _context.Movies.Include(m => m.Genre).ToListAsync();
        }

        public async Task<Movie> GetById(byte id)
        {
           return await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
