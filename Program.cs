using Microsoft.EntityFrameworkCore;
using Grigorova_Server.Data;
using Grigorova_Server.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Grigorova_ServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Grigorova_ServerContext")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BorrowingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
