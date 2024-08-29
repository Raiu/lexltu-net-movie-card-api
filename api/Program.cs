using Api.Data;
using Api.Extensions;
using Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var AllowLocalhost = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: AllowLocalhost,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5173").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlite("Data Source=apidata.db");
});

builder.Services.AddControllerServices();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();

builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseOpenApi();
    app.UseReDoc();
    app.UseCors(AllowLocalhost);

    await app.SeedDataAsync();
}

/* app.UseAuthorization(); */

app.MapControllers();

app.Run();
