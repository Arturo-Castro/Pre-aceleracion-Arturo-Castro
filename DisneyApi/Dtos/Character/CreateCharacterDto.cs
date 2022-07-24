using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DisneyApi.Dtos.Character
{
    public class CreateCharacterDto
    {
        [Required(ErrorMessage = "Image must be specified")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Name must be specified")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age must be specified")]
        public string Age { get; set; }
        [Required(ErrorMessage = "Weight must be specified")]
        public string Weight { get; set; }
        [Required(ErrorMessage = "Biography must be specified")]
        public string Biography { get; set; }

        public List<string> Shows { get; set; }
    }
}
