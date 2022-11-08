using System.ComponentModel.DataAnnotations;

namespace MovieAPIs.DTOs
{
    public class MoviesDTO
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Rate { get; set; }
        [MaxLength(25000)]
        public string Description { get; set; }

        public IFormFile Poster { get; set; } // if using this data type instead of byte will be error in creat action
                                              // because this type is file and cannot store file in array

        //public byte[] Poster { get; set; }
        public byte GenreId { get; set; }

    }
}
