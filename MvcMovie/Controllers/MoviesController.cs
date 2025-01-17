﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Domain;
using MvcMovie.Contracts;
using MvcMovie.Contracts.Movies.Command;
using MediatR;
using MvcMovie.Contracts.Movies.Query;


namespace MvcMovie.Controllers;

public class MoviesController : Controller
{
    private readonly MvcMovieContext _context;
    protected readonly IMediator _mediator;

    public MoviesController(IMediator mediator, MvcMovieContext context)
    {
        _context = context;
        _mediator = mediator;
    }

    // GET: Movies
    public async Task<IActionResult> Index(string SearchString)
    {
        if(_context.Movie == null)
        {
            return Problem("Entity Set Movie is empty, therefore there is nothing to query");
        }
        var movies = from m in _context.Movie select m;
        if (!String.IsNullOrEmpty(SearchString))
        {
            movies = movies.Where(s => s.Title!.ToLower().Contains(SearchString.ToLower()));
        }
        return View(await movies.ToListAsync());
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Description")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Description")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse),StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieCommand command)
    {
        var movie = await _mediator.Send(command);
        var response = Json(movie);
        response.StatusCode = StatusCodes.Status201Created;
        return response;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MovieResponse>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMovies([FromBody]GetAllMoviesQuery request)
    {
        var movies = await _mediator.Send(request);
        var response = Json(movies);
        response.StatusCode = StatusCodes.Status200OK;
        return response;
    }
    [HttpPut]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieCommand command)
    {
        var movieresponse = await _mediator.Send(command);
        var response = Json(movieresponse);
        response.StatusCode = StatusCodes.Status200OK;
        return response;
    }
    [HttpDelete]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteMovie([FromBody] DeleteMovieCommand command)
    {
        var empty_response = await _mediator.Send(command);
        var response = Json(empty_response);
        response.StatusCode = StatusCodes.Status200OK;
        return response;
    }
    [HttpGet]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMovieById([FromBody] GetMovieByIdQuery request)
    {
        var movieresponse = await _mediator.Send(request);
        var response = Json(movieresponse);
        response.StatusCode = StatusCodes.Status200OK;
        return response;
    }
}
