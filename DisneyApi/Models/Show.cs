using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DisneyApi.Models
{
    public class Show
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? DateOfCreation { get; set; }
        public string? Rate { get; set; }

        [ForeignKey("Genre")]
        public int? GenreID { get; set; }
        public ICollection<Character?> Characters { get; set; }
        //public ICollection<Character_Show> character_Shows { get; set; }
        public Genre? Genre { get; set; }
    }
}
