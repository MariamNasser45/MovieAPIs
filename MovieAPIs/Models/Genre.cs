
using System.ComponentModel.DataAnnotations;

namespace MovieAPIs.Models
{
    public class Genre
    {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // to make id generate automatically with using byte type
    public byte Id { get; set; }

        [MaxLength(100)]
    public string Name { get; set; }    

    }
}
