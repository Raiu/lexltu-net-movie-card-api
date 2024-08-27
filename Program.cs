using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>(options => {
    options.UseSqlite("Data Source=apidata.db");
});

builder.Services.AddAutoMapper(
    /* typeof(MappingProfile) */
    typeof(Program)
);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedDataAsync();
}

/* app.UseAuthorization(); */

app.MapControllers();

app.Run();
