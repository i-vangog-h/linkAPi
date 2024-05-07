using Microsoft.EntityFrameworkCore;
using linkApi.DataContext;
using linkApi.Repositories;
using linkApi.Interfaces;
using linkApi.Services;
using linkApi.Factories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<LinkShortenerContext>(options =>
    options.UseNpgsql(_configuration.GetConnectionString("PostgresConnection")),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Transient
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUrlRepo, UrlRepo>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IUrlFactory, UrlFactory>();

/* ############################################################## */ 

var app = builder.Build();

// ure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Map("/", () => { return "index"; });

//app.UseMvc(routes =>
//{
//    routes.MapRoute(
//        name: default,
//        template: "{controller=Home}/{action=Index}/{id?}"
//    );
//});

app.Run();
