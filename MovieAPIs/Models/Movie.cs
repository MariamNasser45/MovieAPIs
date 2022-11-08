using System.ComponentModel.DataAnnotations;

namespace MovieAPIs.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Rate { get; set; }
        [MaxLength(25000)]
        public string Description { get; set; }
         
        public byte[] Poster { get; set; }

        public byte GenreId { get; set; }

        public Genre Genre { get; set; }    
    }
}
