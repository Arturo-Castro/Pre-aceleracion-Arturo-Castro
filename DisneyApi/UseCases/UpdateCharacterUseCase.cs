using DisneyApi.Repositories;
using AutoMapper;
using DisneyApi.Dtos.Character;

namespace DisneyApi.UseCases
{
    public interface IUpdateCharacterUseCase
    {
        Task<EditCharacterDto?> Execute(EditCharacterDto character);
    }

    public class UpdateCharacterUseCase : IUpdateCharacterUseCase
    {
        private readonly DisneyContext _disneyContext;
        private readonly IMapper _mapper;

        public UpdateCharacterUseCase(DisneyContext disneyContext, IMapper mapper)
        {
            _disneyContext = disneyContext;
            _mapper = mapper;
        }

        public async Task<EditCharacterDto?> Execute(EditCharacterDto character)
        {
            var entity =  await _disneyContext.GetCharacter(character.ID);
                       
            if(entity == null)
            {
                return null;
            }

            entity.Image = character.Image;
            entity.Name = character.Name;
            entity.Age = character.Age;
            entity.Weight = character.Weight;
            entity.Biography = character.Biography;

            await _disneyContext.EditCharacter(entity);

            EditCharacterDto editCharacterDto = _mapper.Map<EditCharacterDto>(entity);

            return editCharacterDto;
        }
    }
}
