namespace MvcMovie.Contracts
{
    public record MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public int Price { get; set; }
        public string Rating { get; set; }
        public string? Description { get; set; }
    }
}
