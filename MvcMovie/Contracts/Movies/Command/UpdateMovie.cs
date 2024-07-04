using FluentValidation;
using MediatR;
using MvcMovie.Data;
using MvcMovie.Domain;

namespace MvcMovie.Contracts.Movies.Command;

public record UpdateMovieCommand(
    int Id,
    string? Title,
    string? ReleaseDate,
    string? Genre,
    float? Price,
    string? Rating,
    string? Description) : IRequest<MovieResponse>;

internal class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand,MovieResponse>
{
    private readonly MvcMovieContext _context;
    public UpdateMovieCommandHandler(MvcMovieContext context)
    {
        _context = context;
    }

    public async Task<MovieResponse> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
    {
        var movie_old = await _context.Movie.FindAsync(new object[] { command.Id }, cancellationToken);
        if (command.ReleaseDate != null)
        {
            DateTimeOffset datetimeOffset = DateTimeOffset.Parse(command.ReleaseDate);
            DateTime releaseDate = datetimeOffset.DateTime;
            movie_old.ReleaseDate = releaseDate;
        }
        movie_old.Title = command.Title ?? movie_old.Title;
        movie_old.Rating = command.Rating ?? movie_old.Rating;
        movie_old.Genre = command.Genre;
        movie_old.Price = command.Price ?? movie_old.Price;
        movie_old.Description = command.Description;

        var movie_response = new MovieResponse();

        movie_response.Title = movie_old.Title;
        movie_response.Rating = movie_old.Rating;
        movie_response.Genre = movie_old.Genre;
        movie_response.Price = movie_old.Price;
        movie_response.Description = movie_old.Description;
        await _context.SaveChangesAsync(cancellationToken);
        return movie_response;
    }


}
internal class UpdateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    private readonly MvcMovieContext _context;

    public UpdateMovieCommandValidator(MvcMovieContext context)
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

