using System.ComponentModel.DataAnnotations;

namespace MovieAPIs.DTOs
{
    // this class to controlling which movie deatails appear when using Fet Method
    public class MovieDeatialsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
        public byte[] Poster { get; set; }
        public byte GenreId { get; set; }
        public string GenreName { get; set; }

        
    }
}
