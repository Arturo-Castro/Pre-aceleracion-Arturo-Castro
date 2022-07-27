using AutoMapper;
using DisneyApi.Dtos.Character;
using DisneyApi.Dtos.Show;
using DisneyApi.Models;

namespace DisneyApi.Profiles
{
    public class CharacterAndShowProfile : Profile
    {
        public CharacterAndShowProfile()
        {
            CreateMap<Character, CharacterDto>();
            CreateMap<Character, CharactersDto>();
            CreateMap<Character, EditCharacterDto>();
            CreateMap<Character, ShowsCharactersDto>();            
            CreateMap<CreateCharacterDto, Character>();
            CreateMap<Show, CharactersShows>();
            CreateMap<Show, EditShowDto>();
            CreateMap<ShowTitleDto, Show>();
            CreateMap<Show, ShowDto>();
        }
    }
}
