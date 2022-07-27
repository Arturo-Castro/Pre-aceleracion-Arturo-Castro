using DisneyApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DisneyApi.Dtos.Character;
using DisneyApi.Dtos.Show;
using System.ComponentModel.DataAnnotations;

namespace DisneyApi.Repositories
{
    public class DisneyContext : DbContext
    {
        private readonly IMapper _mapper;
        public DisneyContext(DbContextOptions<DisneyContext> options, IMapper mapper) : base(options) 
        {
            _mapper = mapper;
        }

        public DbSet<Character> Characters { get; set; }      
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Show> Shows { get; set; }
        

        public async Task<Character?> GetCharacterAndShow(int id)
        {
            return await Characters
                .Include(x => x.Shows)                
                .FirstOrDefaultAsync(y => y.ID == id);
        }

        public async Task<Show?> GetShowAndCharacter(int id)
        {
            return await Shows
                .Include(x => x.Characters)
                .FirstOrDefaultAsync(y => y.ID == id);
        }

        public async Task<Character?> GetCharacter(int id)
        {   
            return await Characters.FirstOrDefaultAsync(y => y.ID == id);
        }

        public async Task<bool> DeleteCharacter(int id)
        {
            Character? entity = await GetCharacter(id);

            if (entity == null) 
            { 
                return false; 
            }

            Characters.Remove(entity);            
            SaveChanges();
            return true;

        }

        public async Task<Show?> GetShow(int id)
        {
            return await Shows.FirstOrDefaultAsync(y => y.ID == id);
        }

        public async Task<bool> DeleteShow(int id)
        {
            Show? entity = await GetShow(id);

            if (entity == null)
            {
                return false;
            }

            Shows.Remove(entity);
            SaveChanges();
            return true;

        }

        public async Task<bool> EditCharacter(Character character)
        {
            Characters.Update(character);
            await SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditShow(Show show)
        {
            Shows.Update(show);
            await SaveChangesAsync();

            return true;
        }

        public async Task<Character?> AddCharacter(CreateCharacterDto createCharacterDto)
        {
            Character character = new Character()
            {
                ID = null,
                Image = createCharacterDto.Image,
                Name = createCharacterDto.Name,
                Age = createCharacterDto.Age,
                Weight = createCharacterDto.Weight,
                Biography = createCharacterDto.Biography,
            };

            var response = await Characters.AddAsync(character);
            await SaveChangesAsync();

            var characterAndShows = await Characters.Include(p => p.Shows).FirstOrDefaultAsync(p => p.Name == createCharacterDto.Name);

            foreach (string show in createCharacterDto.Shows)
            {                
                Show show1 = new Show()
                {
                    Title = show,
                };

                await Shows.AddAsync(show1);
                await SaveChangesAsync();

                var singleShow = await Shows.FirstOrDefaultAsync(p => p.Title == show);
                characterAndShows.Shows.Add(singleShow);
                await SaveChangesAsync();
            };

            return await GetCharacter(response.Entity.ID ?? throw new Exception("Could not save"));
        }

        public async Task<Show?> AddShow(CreateShowDto createShowDto)
        {
            Show show = new Show()
            {
                ID = null,
                Image = createShowDto.Image,
                Title = createShowDto.Title,
                DateOfCreation = createShowDto.DateOfCreation,
                Rate = createShowDto.Rate,
                GenreID = createShowDto.GenreId,
            };

            var response = await Shows.AddAsync(show);
            await SaveChangesAsync();

            var showAndCharacters = await Shows.Include(p => p.Characters).FirstOrDefaultAsync(p => p.Title == createShowDto.Title);

            foreach (string character in createShowDto.Characters)
            {
                Character character1 = new Character()
                {
                    Name = character,
                };

                await Characters.AddAsync(character1);
                await SaveChangesAsync();

                var singleCharacter = await Characters.FirstOrDefaultAsync(p => p.Name == character);
                showAndCharacters.Characters.Add(singleCharacter);
                await SaveChangesAsync();
            };

            return await GetShow(response.Entity.ID ?? throw new Exception("Could not save"));
        }
        
        public async Task<List<Character>> GetCharactersByName(string name)
        {
            return await Characters.Where(c => c.Name == name).ToListAsync();
        }

        public async Task<List<Character>> GetCharactersByAge(string age)
        {
            return await Characters.Where(c => c.Age == age).ToListAsync();
        }

        public async Task<List<Character>> GetCharactersByWeight(string weight)
        {
            return await Characters.Where(c => c.Weight == weight).ToListAsync();
        }

        public async Task<List<Show>> GetShowsByTitle(string title)
        {
            return await Shows.Where(m => m.Title == title).ToListAsync();
        }

        public async Task<List<Show>> GetShowsByGenreId(int genreId)
        {
            return await Shows.Where(m => m.GenreID == genreId).ToListAsync();
        }

        public async Task<List<Show>> GetShowsByOrder(string order)
        {
            var shows = new List<Show>();
            if (order == "ASC")
                shows = await (from c in Shows
                                        orderby c.DateOfCreation ascending
                                        select c).ToListAsync();
            if (order == "DESC")
                shows = await (from c in Shows
                                        orderby c.DateOfCreation descending
                                        select c).ToListAsync();

            return shows;
        }
    }
}
