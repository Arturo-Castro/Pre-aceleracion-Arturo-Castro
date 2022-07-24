using AutoMapper;
using DisneyApi.Dtos.Show;
using DisneyApi.Repositories;

namespace DisneyApi.UseCases
{
    public interface IUpdateShowUseCase
    {
        Task<EditShowDto?> Execute(EditShowDto show);
    }

    public class UpdateShowUseCase : IUpdateShowUseCase
    {
        private readonly DisneyContext _disneyContext;
        private readonly IMapper _mapper;
                   
        public UpdateShowUseCase(DisneyContext disneyContext, IMapper mapper)
        {
            _disneyContext = disneyContext;
            _mapper = mapper;
        }

        public async Task<EditShowDto?> Execute(EditShowDto show)
        {
            var entity = await _disneyContext.GetShow(show.ID);

            if (entity == null)
            {
                return null;
            }

            entity.Image = show.Image;
            entity.Title = show.Title;
            entity.DateOfCreation = show.DateOfCreation;
            entity.Rate = show.Rate;
            entity.GenreID = show.GenreID;                

            await _disneyContext.EditShow(entity);

            EditShowDto editShowDto = _mapper.Map<EditShowDto>(entity);

            return editShowDto;
        }
        
    }
}
