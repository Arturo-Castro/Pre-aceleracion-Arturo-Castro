using System.ComponentModel.DataAnnotations;

namespace DisneyApi.Dtos.Show
{
    public class CreateShowDto
    {
        [Required(ErrorMessage = "Image must be specified")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Title must be specified")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Date must be specified")]
        public string DateOfCreation { get; set; }
        [Required(ErrorMessage = "Rate must be specified")]
        public string Rate { get; set; }       
        public int? GenreId { get; set; }

        public List<string> Characters { get; set; }
    }
}
