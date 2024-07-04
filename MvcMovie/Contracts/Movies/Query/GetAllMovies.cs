namespace MvcMovie.Contracts.Movies.Query;

public record GetAllMovies
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string Genre { get; set; }
    public float Price { get; set; }

}
#region Query 
public class GetAllMoviesQuery { }

#endregion Query
#region Validator
internal class GetAllMoviesValidator { }

#endregion Validator