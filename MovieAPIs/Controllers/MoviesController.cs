using Microsoft.AspNetCore.Components;

namespace MovieAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // add new movies

        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        // in movies also using DTO to recieve data from user and put them in DB instead of define then in this method
        public async Task<IActionResult> AddMovieAsync([FromForm]MoviesDTO dto) // using from form to define the poster is form not string as appear in swagger when not use from form 
        {
            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream); // casting from file to array

            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                //Poster = dto.Poster,
                Poster = datastream.ToArray(),
                Rate = dto.Rate,
                Description = dto.Description,
                Year = dto.Year,

            };
            await _context.AddAsync(movie);
            _context.SaveChangesAsync();
            return Ok(movie);
        }

    }
}
