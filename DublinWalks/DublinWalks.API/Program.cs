using DublinWalks.API.Data;
using DublinWalks.API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DublinDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DublinWalks"));
});

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
//Dependancy injection for Walk Repository
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
//Dependancy injection for WalkDifficulty Repository
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
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
