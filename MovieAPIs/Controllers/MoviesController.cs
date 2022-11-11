///using AutoMapper;
using Microsoft.AspNetCore.Components;
using MovieAPIs.DTOs;
using MovieAPIs.Migrations;
using MovieAPIs.Models;
using System.Data.Common;

namespace MovieAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // determine types and size of files which user entered it befor apply request

        private new List<string> _allowedextentions = new List<string> { ".jpg", ".png" };

        private long _Maxsize = 5242880; // enter size with byte type using any site to convert from MG=>Byte https://www.gbmb.org/mb-to-bytes

        //private readonly IMapper _autoMap;
        public MoviesController(ApplicationDbContext context /*, IMapper autoMap*/)
        {
            _context = context;
            //_autoMap = autoMap;

        }

        [HttpGet]  // not define name of it becaus it is only get method in this class
        public async Task<IActionResult> GetAllMovieAsync()
        {
            //in this code returne genre data as complex but if we need to returne only this its name we define it in movieDTO
            //"genre": {
            //    "id": 6,
            //"name": "Animy"

            //var movie = await _context.Movies.Include(m => m.Genre).ToListAsync();//using Include(m => m.Genre): to avoid returne genre=null so using Include(m => m.Genre) to return genre data 
            //return Ok(movie);

            //>>>>>>>>> syntax if : we need to returne only specific properties which we need 
            //Create new class to define need properties in it but better way is we make base class using as generic or using DTO

            var movie = await _context.Movies.Include(m => m.Genre).
                OrderByDescending(m => m.Rate)
                .Select(m => new MovieDeatialsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Description = m.Description,
                    Rate = m.Rate,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    Poster = m.Poster

                }).ToListAsync();  //using Include(m => m.Genre): to avoid returne genre=null so using Include(m => m.Genre) to return genre data 
            return Ok(movie);

        }

        //method to returne all data of movie which recieve its id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre)
                .SingleOrDefaultAsync(m=> m.Id == id);

            if(movie == null)
                return NotFound("Enter correct Id");
            var dto = new MovieDeatialsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Description = movie.Description,
                Rate = movie.Rate,
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,
                Poster = movie.Poster
            };
            return Ok(dto);
        }


        // create get method which recieve GenreId and return only movies which have this genreid

        [HttpGet("GenreId")] // using id , GetByGenreId ..... etc to avoid error of httpget when define multiple method httpget
        public async Task<IActionResult> GetByGenreId(byte genreid)
        {
            var movie = await _context.Movies.
                Where(m => m.GenreId == genreid).
                OrderByDescending(r => r.Rate)
                .Include(m => m.Genre)
                .Select(m => new MovieDeatialsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Description = m.Description,
                    Rate = m.Rate,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    Poster = m.Poster

                }).ToListAsync();  //using Include(m => m.Genre): to avoid returne genre=null so using Include(m => m.Genre) to return genre data 
            return Ok(movie);

        }



        // add new movies
        [HttpPost]
        // in movies also using DTO to recieve data from user and put them in DB instead of define then in this method
        public async Task<IActionResult> AddMovieAsync([FromForm] MoviesDTO dto) // using from form to define the poster is form not string as appear in swagger when not use from form 
        {
            // cheack extention , size of file
            if (!_allowedextentions.Contains(Path.GetExtension(dto.Poster.FileName))) // if exe not exist in exe defined then apply next code
            {
                return BadRequest("This Exe Not allowed please enter file with onlt .jpg , .png");

                if (dto.Poster.Length > _Maxsize)
                    return BadRequest("Please enter file not exceed 3 MG");
            }

            var IsvallideGenreid = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!IsvallideGenreid)
                return BadRequest(" Not exist GenreID");
            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream); // casting from file to array

            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                //Poster = dto.Poster, line 17 in DTO
                Poster = datastream.ToArray(),
                Rate = dto.Rate,
                Description = dto.Description,
                Year = dto.Year,

            };

            //            //Using AutoMapper

            //            //var movie = _autoMap.Map<Movie>(dto);
            //            //movie.Poster = datastream.ToArray();

            _context.AddAsync(movie);
            _context.SaveChangesAsync();
            return Ok(movie);
        }



        [HttpPut("{id}")]  //to user can add need id at the end of url as: "/api/Genres/id"
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MoviesDTO dto) // (id of required movie to update , [FromBody] : data to edit , using frombody to define the data recieved from body not from Url)
        {
            // cheack id  of movie & genre are exist , true or not

            var movieid = await _context.Movies.FindAsync(id);

            if (movieid == null)
                return BadRequest($"There Are No Any Movie with Id : {id}");


            var IsvallideGenreid = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!IsvallideGenreid)
                return BadRequest(" Not exist GenreID");

            if(dto.Poster != null) // in case user need to update poster then check enter poster condition 
            {
                // cheack extention , size of file
                if (!_allowedextentions.Contains(Path.GetExtension(dto.Poster.FileName))) // if exe not exist in exe defined then apply next code
                {
                    return BadRequest("This Exe Not allowed please enter file with onlt .jpg , .png");

                    if (dto.Poster.Length > _Maxsize)
                        return BadRequest("Please enter file not exceed 3 MG");
                }

                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream); // casting from file to array
                movieid.Poster = datastream.ToArray();
            }

            // Define Properties which user can change them

            //movieid.Title = dto.Title;
            movieid.Year = dto.Year;
            movieid.GenreId = dto.GenreId;
            movieid.Rate = dto.Rate;
            movieid.Description = dto.Description;

            _context.SaveChanges();

            return Ok(movieid);


        }

         //// Delete movies
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return BadRequest($"There Are No Any Movie with Id : {id}");

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Ok($"Movie {movie.Title} Deleted Successfully");


        }

    }
}
