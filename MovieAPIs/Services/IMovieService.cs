namespace MovieAPIs.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMovie(); //endpoint to returne all Movies data exist in DB "get method"
        Task<Movie> GetById(byte id); // because the update method contain syntax which rturn ONLY  one item from table in DB
        Task<Movie> Add(Movie Movie);   //end point of add "post method"
        Movie Update(Movie movie); // put method

        Movie Delete(Movie movie);

    }
}
