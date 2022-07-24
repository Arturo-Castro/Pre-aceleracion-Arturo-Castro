using DisneyApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DisneyApi.Dtos.Character;
using DisneyApi.Dtos.Show;

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

        public async Task<bool> EditCharacter(Character character)
        {
            Characters.Update(character);
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
    }
}
