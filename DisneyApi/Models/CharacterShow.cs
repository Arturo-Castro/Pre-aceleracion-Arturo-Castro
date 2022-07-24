using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisneyApi.Models
{
    public class CharacterShow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("Character")]
        public int? CharactersID { get; set; }

        [ForeignKey("Show")]
        public int? ShowsID { get; set; }

        public Character Character { get; set; }
        public Show Show { get; set; }
    }
}
