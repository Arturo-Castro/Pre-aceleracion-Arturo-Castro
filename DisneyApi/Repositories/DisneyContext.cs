using DisneyApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DisneyApi.Dtos.Character;
using DisneyApi.Dtos.Show;

namespace DisneyApi.Repositories
{
    public class DisneyContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }      
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Show> Shows { get; set; }
    }
}
