﻿

namespace MovieAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        //frist endpoint of genres table : using get method to return all genres exist in DB

        private readonly ApplicationDbContext _context; // to work with DB

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]  // not define name of it becaus it is only get method in this class
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();//using OrderBy : postman by default arrange genres by ID but the arrange them by alphabatic is the better 
            return Ok(genres);

        }

        //end point to add new genres

        [HttpPost]
        public async Task<IActionResult> CreateGenresAsyns(CreateGenreDTO dto)
        {
            // add item in Db by recieve name of genre only from user request 
            // id is automatically generated 

            // using DTO which  responisble for only moving data 

            var genre = new Genre { Name = dto.Name };

            await _context.AddAsync(genre);
            _context.SaveChanges();
            return Ok(genre); // Ok(genre) to return genre name 

        }

        // to edit, update genres data

        [HttpPut("{id}")]  //to user can add need id at the end of url as: "/api/Genres/id"
        public async Task<IActionResult> UpdateGenres(int id, [FromBody] CreateGenreDTO dto) // (id of required genres to update , [FromBody] : data to edit , using frombody to define the data recieved from body not from Url)
        {
            // cheack id exist , true or not

            var genreid = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if (genreid == null)
                return BadRequest($"There Are No Any Genres with Id : {id}");

            genreid.Name = dto.Name;

            _context.SaveChanges();

            return Ok(genreid);


        }

        // Delete genres 
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeletGenre(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                return BadRequest($"There Are No Any Genres with Id : {id}");

            _context.Genres.Remove(genre);
            _context.SaveChanges();

            return Ok($"Genre {genre.Name} Deleted Successfully");


        }

    }
}
