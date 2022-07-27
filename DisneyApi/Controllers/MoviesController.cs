using AutoMapper;
using DisneyApi.Dtos.Character;
using DisneyApi.Dtos.Show;
using DisneyApi.Models;
using DisneyApi.Repositories;
using DisneyApi.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisneyApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    [Authorize]
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

        [HttpGet("title")]
        public async Task<ActionResult<List<ShowDto>>> GetShowsByTitle(string title)
        {

            var movies = await _disneyContext.GetShowsByTitle(title);
            var result = new List<ShowDto>();

            foreach (var movie in movies)
            {
                ShowDto showDto = _mapper.Map<ShowDto>(movie);

                result.Add(showDto);
            }

            return Ok(result);
        }
       
        [HttpGet("genre")]
        public async Task<ActionResult<List<ShowDto>>> GetShowsByGenreId(int genreId)
        {

            var movies = await _disneyContext.GetShowsByGenreId(genreId);
            var result = new List<ShowDto>();

            foreach (var movie in movies)
            {
                ShowDto showDto = _mapper.Map<ShowDto>(movie);

                result.Add(showDto);
            }

            return Ok(result);
        }

        [HttpGet("order")]
        public async Task<ActionResult<List<ShowDto>>> GetShowsByOrder(string order)
        {
            order = order.ToUpper();

            var movies = await _disneyContext.GetShowsByOrder(order);
            var result = new List<ShowDto>();

            foreach (var movie in movies)
            {
                ShowDto showDto = _mapper.Map<ShowDto>(movie);

                result.Add(showDto);
            }

            return Ok(result);
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

