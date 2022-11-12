

using Microsoft.EntityFrameworkCore;
using MovieAPIs.Services;

namespace MovieAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        //frist endpoint of genres table : using get method to return all genres exist in DB

        //private readonly ApplicationDbContext _context; // to work with DB : after adding services dont need it in this class


        private readonly IGenresService _genreservices; //replace context ctor 

        public GenresController(IGenresService genreservices)
        {
            _genreservices = genreservices;
        }

        

        [HttpGet]  // not define name of it becaus it is only get method in this class
        public async Task<IActionResult> GetAllAsync()
        {
            //var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();//using OrderBy : postman by default arrange genres by ID but the arrange them by alphabatic is the better 

            ////////////////////////////////////////////////////////////////////////////////////
            //AFTER USING SERVICES 

            var genres = await _genreservices.GetAllGenre();
            return Ok(genres);

        }

        //end point to add new genres

        [HttpPost]
        public async Task<IActionResult> CreateGenresAsyns(CreateGenreDTO dto)
        {
            //// add item in Db by recieve name of genre only from user request 
            //// id is automatically generated 

            //// using DTO which  responisble for only moving data 

            //var genre = new Genre { Name = dto.Name };

            //await _context.AddAsync(genre);
            //_context.SaveChanges();

            ////////////////////////////////////////////////////////////////////////////////////
          
            //AFTER USING SERVICES 
            var genre = new Genre { Name = dto.Name };

            await _genreservices.Add(genre);

            return Ok(genre); // Ok(genre) to return genre name 

        }

        // to edit, update genres data

        [HttpPut("{id}")]  //to user can add need id at the end of url as: "/api/Genres/id"
        public async Task<IActionResult> UpdateGenres(byte id, [FromBody] CreateGenreDTO dto) // (id of required genres to update , [FromBody] : data to edit , using frombody to define the data recieved from body not from Url)
        {

            // cheack id exist , true or not

            //var genreid = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            //if (genreid == null)
            //    return BadRequest($"There Are No Any Genres with Id : {id}");

            //genreid.Name = dto.Name;

            //_context.SaveChanges();

            ////////////////////////////////////////////////////////////////////////////////////
            //AFTER USING SERVICES 

            var genreid = await _genreservices.GetById(id);

            if (genreid == null)
                return BadRequest($"There Are No Any Genres with Id : {id}");
  
            genreid.Name = dto.Name;

            _genreservices.Update(genreid);
            return Ok(genreid);


        }

        // Delete genres 
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeletGenre(byte id) // change id type int=>byte because of it defined in services of type byte 
        {
            //var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            //if (genre == null)
            //    return BadRequest($"There Are No Any Genres with Id : {id}");

            //_context.Genres.Remove(genre);
            //_context.SaveChanges();

            ////////////////////////////////////////////////////////////////////////////////////
            //AFTER USING SERVICES 

            var genre = await _genreservices.GetById(id);

            if (genre == null)
                return BadRequest($"There Are No Any Genres with Id : {id}");

            _genreservices.Delete(genre);
            return Ok($"Genre {genre.Name} Deleted Successfully");


        }

    }
}
