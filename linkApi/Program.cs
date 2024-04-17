using Microsoft.EntityFrameworkCore;
using linkApi.DataContext;
using linkApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<LinkShortenerContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Transient
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UrlRepo, UrlRepo>();

var app = builder.Build();

// ure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Map("/", () => { return "index"; });

app.Run();
