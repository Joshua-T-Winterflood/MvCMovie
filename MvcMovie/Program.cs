using FluentValidation;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MvcMovie.Data;
using System.Reflection;
using Weasel.Core;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MvcMovieContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});



///////////////////////////////////////////////// MY SECTION //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//builder.Services.AddMarten(sp =>
//{
//    var options = new StoreOptions();
//    options.Connection(ConnectionSource.ConnectionString);
//    var martenSettings = sp.GetRequiredService<IOptions<MartenSettings>>().Value;

//    if (!string.IsNullOrEmpty(martenSettings.SchemaName))
//    {
//        options.Events.DatabaseSchemaName = martenSettings.SchemaName;
//        options.DatabaseSchemaName = martenSettings.SchemaName;
//    }

//    return options;
//}).UseLightweightSessions();




///////////////////////////////////////////////// MY SECTION //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
