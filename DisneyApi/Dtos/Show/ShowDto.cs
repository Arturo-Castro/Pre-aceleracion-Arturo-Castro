namespace DisneyApi.Dtos.Show
{
    public class ShowDto
    {
        public int? ID { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string DateOfCreation { get; set; }
        public string? Rate { get; set; }
        public int? GenreID { get; set; }

        public List<ShowsCharactersDto> Characters { get; set; }
    }
}
