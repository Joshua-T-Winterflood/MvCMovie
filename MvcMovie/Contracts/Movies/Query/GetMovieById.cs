using MediatR;
using MvcMovie.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MvcMovie.Contracts.Movies.Query;
public record GetMovieByIdQuery(
    int Id
    ) : IRequest<MovieResponse>;


internal class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse>
{
    private readonly MvcMovieContext _context;
    public GetMovieByIdQueryHandler(MvcMovieContext context)
    {
        _context = context;
    }
    public async Task<MovieResponse> Handle( GetMovieByIdQuery request , CancellationToken cancellationToken)
    {
        var movie = await _context.Movie.FindAsync(new object[] { request.Id }, cancellationToken);
        var movie_response = new MovieResponse();

        movie_response.Title = movie.Title;
        movie_response.Rating = movie.Rating;
        movie_response.Genre = movie.Genre;
        movie_response.Price = movie.Price;
        movie_response.Description = movie.Description;
        movie_response.Id = movie.Id;

        return movie_response;
    }
}
    
