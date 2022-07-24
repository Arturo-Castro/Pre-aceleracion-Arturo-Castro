using AutoMapper;
using DisneyApi.Dtos.Character;
using DisneyApi.Dtos.Show;
using DisneyApi.Models;
using DisneyApi.Repositories;
using DisneyApi.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace DisneyApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly DisneyContext _disneyContext;
        private readonly IMapper _mapper;
        private readonly IUpdateShowUseCase _updateShowUseCase;

        public MoviesController(DisneyContext disneyContext, IMapper mapper, IUpdateShowUseCase updateShowUseCase)
        {
            _disneyContext = disneyContext;
            _mapper = mapper;
            _updateShowUseCase = updateShowUseCase;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ShowsDto>))]
        public async Task<IActionResult> GetShows()
        {
            var response = _disneyContext.Shows.Select(x => new ShowsDto
            {
                Image = x.Image,
                Title = x.Title,
                DateOfCreation = x.DateOfCreation,
            });

            return new OkObjectResult(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShowDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShow(int id)
        {
            var show = await _disneyContext.GetShowAndCharacter(id);

            if (show == null)
            {
                return NotFound();
            }

            ShowDto showDto = _mapper.Map<ShowDto>(show);

            return new OkObjectResult(showDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var response = await _disneyContext.DeleteShow(id);

            if (!response)
            {
                return NotFound();
            }

            return new OkObjectResult(response);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateShowDto))]
        public async Task<IActionResult> CreateShow(CreateShowDto show)
        {
            Show? response = await _disneyContext.AddShow(show);

            return new CreatedResult($"https://localhost:7106/api/movies/{response.ID}", null);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditCharacterDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateShow(EditShowDto show)
        {
            EditShowDto? response = await _updateShowUseCase.Execute(show);

            if (response == null)
            {
                return NotFound();
            }

            return new OkObjectResult(response);
        }
    }
}

