using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace MvcMovie.Contracts.Movies.Query;
#region Helpers
public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int Offset { get; set; }
    public int Count { get; set; }
}
public static class PagedResultExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int? offset = null,
        int? limit = null,
        CancellationToken cancellation = default)
    {
        var count = await query.CountAsync(cancellation);

        if (offset != null)
            query = query.Skip(offset.Value);

        if (limit != null)
            query = query.Take(limit.Value);

        var data = await query.ToListAsync(cancellation);

        return new PagedResult<T>
        {
            Data = data,
            Count = count,
            Offset = offset ?? 0,
        };
    }
}

    #endregion Helpers

public record GetAllMoviesQuery(
        int? Offset = null,
        int? Limit = null
) : IRequest<PagedResult<MovieResponse>>;



#region Query 
internal class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, PagedResult<MovieResponse>>
{
    private readonly MvcMovieContext _context;

    public GetAllMoviesQueryHandler(MvcMovieContext context)
    {
        _context = context;
    }
    public async Task<PagedResult<MovieResponse>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        
        // Execute the query and get the list of movies
        var query = _context.Movie.AsNoTracking();

        // Utilize ToPagedResultAsync extension method to get the paged result
        var pagedResult = await query.Select(movie => new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Price = movie.Price,
            Description = movie.Description,
            Rating = movie.Rating
        }).ToPagedResultAsync(request.Offset, request.Limit, cancellationToken);

        return pagedResult;
    }
}


#endregion Query