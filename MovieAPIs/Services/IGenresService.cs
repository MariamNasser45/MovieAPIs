                      // Interface of all end points are defined in Genres controller to improve code

namespace MovieAPIs.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAllGenre(); //endpoint to returne all genres data exist in DB "get method"
        Task<Genre> GetById(byte id); // because the update method contain syntax which rturn ONLY  one item from table in DB
        Task<Genre> Add(Genre genre);   //end point of add "post method"
        Genre Update(Genre genre); // put method
        Genre Delete(Genre genre); 
        Task<bool> IsvallideGenreid(byte id);   // to usyng IsvallideGenreid in add movie method 


    }
}
