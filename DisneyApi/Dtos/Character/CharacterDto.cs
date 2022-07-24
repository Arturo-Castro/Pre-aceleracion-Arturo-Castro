using DisneyApi.Dtos.Show;
using DisneyApi.Models;

namespace DisneyApi.Dtos.Character
{
    public class CharacterDto
    {
        public int? ID { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? Weight { get; set; }
        public string? Biography { get; set; }

        public List<CharactersShows> Shows { get; set; }
    }
}
