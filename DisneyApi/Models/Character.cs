using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DisneyApi.Dtos;

namespace DisneyApi.Models
{
    public class Character        
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Biography { get; set; }

        public ICollection<Show> Shows { get; set; }                
    }
}
