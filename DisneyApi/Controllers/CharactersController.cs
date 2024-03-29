﻿using AutoMapper;
using DisneyApi.Dtos.Character;
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
    public class CharactersController : Controller
    {
        private readonly DisneyContext _disneyContext;
        private readonly IMapper _mapper;
        private readonly IUpdateCharacterUseCase _updateCharacterUseCase;

        public CharactersController(DisneyContext disneyContext, IMapper mapper, IUpdateCharacterUseCase updateCharacterUseCase)
        {
            _disneyContext = disneyContext;
            _mapper = mapper;
            _updateCharacterUseCase = updateCharacterUseCase;
        }

        [HttpGet("name")]
        public async Task<ActionResult<List<CharacterDto>>> GetCharactersByName(string name)
        {
            var characters = await _disneyContext.GetCharactersByName(name);
            var result = new List<CharacterDto>();

            foreach (var character in characters)
            {
                CharacterDto characterDto = _mapper.Map<CharacterDto>(character);
                
                result.Add(characterDto);
            }

            return Ok(result);
        }

        [HttpGet("age")]
        public async Task<ActionResult<List<CharacterDto>>> GetCharactersByAge(string age)
        {

            var characters = await _disneyContext.GetCharactersByAge(age);
            var result = new List<CharacterDto>();

            foreach (var character in characters)
            {
                CharacterDto characterDto = _mapper.Map<CharacterDto>(character);

                result.Add(characterDto);
            }

            return Ok(result);
        }

        [HttpGet("weight")]
        public async Task<ActionResult<List<CharacterDto>>> GetCharactersByWeight(string weight)
        {

            var characters = await _disneyContext.GetCharactersByWeight(weight);
            var result = new List<CharacterDto>();

            foreach (var character in characters)
            {
                CharacterDto characterDto = _mapper.Map<CharacterDto>(character);

                result.Add(characterDto);
            }

            return Ok(result);
        }

        [HttpGet("movies")]
        public async Task<ActionResult<List<Character>>> GetCharactersByMovieId(int movieId)
        {
            var characters = await _disneyContext.Shows.Where(m => m.ID == movieId).Select(m => new { Title = m.Title, Characters = m.Characters })
            .ToListAsync();

            return Ok(characters);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CharactersDto>))]
        public async Task<IActionResult> GetCharacters()
        {
            var response = _disneyContext.Characters.Select(x => new CharactersDto
            {
                Image = x.Image,
                Name = x.Name,
            });

            return new OkObjectResult(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacterDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCharacter (int id)
        {
            var character = await _disneyContext.GetCharacterAndShow(id);

            if (character == null)
            {
                return NotFound();
            }
            
            CharacterDto characterDto = _mapper.Map<CharacterDto>(character);            

            return new OkObjectResult(characterDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var response = await _disneyContext.DeleteCharacter(id);

            if (!response)
            {
                return NotFound();
            }

            return new OkObjectResult(response);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCharacterDto))]
        public async Task<IActionResult> CreateCharacter(CreateCharacterDto character)
        {
            Character? response = await _disneyContext.AddCharacter(character);

            return new CreatedResult($"https://localhost:7106/api/characters/{response.ID}", null);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditCharacterDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCharacter(EditCharacterDto character)
        {
            EditCharacterDto? response = await _updateCharacterUseCase.Execute(character);

            if(response == null)
            {
                return NotFound();
            }

            return new OkObjectResult(response);
        }
    }
}
