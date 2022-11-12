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

        public async Task<IEnumerable<Movie>> GetAllMovie(byte genreId=0)
        {
            return await _context.Movies
                .Where(m => m.GenreId == genreId || genreId==0) // if user sent exist genere id in db then return all movies of its id
                .Include(m => m.Genre).ToListAsync();          // OR if id still 0 "default value" not return anything

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
