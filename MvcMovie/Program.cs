using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MvcMovieContext")));
//options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{SearchString?}");

app.Run();
