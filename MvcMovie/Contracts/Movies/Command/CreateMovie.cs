using MvcMovie.Mediator;
using NuGet.Protocol.Plugins;
using MvcMovie.Domain;
using MvcMovie.Data;

namespace MvcMovie.Contracts.Movies.Command;

public record CreateMovieCommand(
    int MovieId,
    string Title,
    string ReleaseDate,
    string? Genre,
    float Price,
    string Rating,
    string? Description)
    : IRequest<MovieResponse, IReportingEntity<Movie>>;

//#region Query Section
//internal class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieResponse>
//{   

//    private readonly MvcMovieContext _context;
//    public CreateMovieCommandHandler(MvcMovieContext context)

//    {
//        _context = context;
//    }

//    public async Task<MovieResponse> Handle(CreateMovieCommand command)
//    {
//        var movie = new Movie();

//        if (string.IsNullOrEmpty(command.Genre))
//        {
//            movie.Genre = null;
//        }
//        if (string.IsNullOrEmpty(command.Description))
//        {
//            movie.Description = null;
//        }
//    }
//}
    

