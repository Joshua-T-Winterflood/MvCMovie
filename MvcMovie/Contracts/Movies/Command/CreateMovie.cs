using FluentValidation;
using MediatR;
using MvcMovie.Data;
using MvcMovie.Domain;

namespace MvcMovie.Contracts.Movies.Command;

public record CreateMovieCommand(
    string Title,
    string ReleaseDate,
    string? Genre,
    float Price,
    string Rating,
    string? Description)
    : IRequest<MovieResponse>;

#region Query Handler
internal class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieResponse>
{   

    private readonly MvcMovieContext _context;
    public CreateMovieCommandHandler(MvcMovieContext context)

    {
        _context = context;
    }

    public async Task<MovieResponse> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
    {
        DateTimeOffset datetimeOffset = DateTimeOffset.Parse(command.ReleaseDate);
        DateTime releaseDate = datetimeOffset.DateTime;

        var movie = new Movie(command.Title, releaseDate, command.Genre, command.Price, command.Description, command.Rating);

        if (string.IsNullOrEmpty(command.Genre))
        {
            movie.Genre = null;
        }
        if (string.IsNullOrEmpty(command.Description))
        {
            movie.Description = null;
        }


        _context.Movie.Add(movie);
        await _context.SaveChangesAsync(cancellationToken);

        var movieResponse = new MovieResponse();

        movieResponse.Id = movie.Id;
        movieResponse.Title = movie.Title;
        movieResponse.Price = (float)movie.Price;
        movieResponse.Rating = movie.Rating;
        movieResponse.Genre = movie.Genre;
        movieResponse.Description = movie.Description;
        movieResponse.ReleaseDate = movie.ReleaseDate;

        return movieResponse;

    }

}
#endregion Query Handler    
#region Query Validator
internal class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    private readonly MvcMovieContext _context;

    public CreateMovieCommandValidator(MvcMovieContext context)
    {
        RuleFor(r => r.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(r => r.Price)
            .Must(HaveNotMoreThanTwoDecimals)
            .WithMessage("Price cannot have more than 2 decimal places.");
    }

    private bool HaveNotMoreThanTwoDecimals(float price)
    {
        string strValue = price.ToString();
        var parts = strValue.Split('.');
        
        if (parts.Length == 2)
        {
            return parts[1].Length <= 2;
        }
        return true;
    }
}
#endregion Query Validator