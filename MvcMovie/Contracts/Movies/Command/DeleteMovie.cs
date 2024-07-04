using MediatR;
using MvcMovie.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace MvcMovie.Contracts.Movies.Command;

public record DeleteMovieCommand(
    int Id
    ): IRequest<Unit>;

internal class DeleteMoviecommandHandler : IRequestHandler<DeleteMovieCommand, Unit>
{
    private readonly MvcMovieContext _context;

    public DeleteMoviecommandHandler(MvcMovieContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteMovieCommand command, CancellationToken cancellationToken)
    {
        var movie = await _context.Movie.FindAsync(new object[] { command.Id }, cancellationToken);

        if (movie == null)
        {
            throw new InvalidCastException("The movie you tried to delete from the database doesnt exist");
        }
        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
#region Validator
internal class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    private readonly MvcMovieContext _context;

    public DeleteMovieCommandValidator(MvcMovieContext context)
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .NotNull();
    }

}

#endregion Validator



