using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Domain;

public class Movie
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string? Genre { get; set; }
    public float Price { get; set; }
    public string? Description { get; set; }
    public string Rating { get; set; }

    public Movie(string title, DateTime releaseDate, string genre, float price, string description, string rating)
    {
        Title = title;
        ReleaseDate = releaseDate;
        Genre = genre;
        Price = price;
        Description = description;
        Rating = rating;
    }

}