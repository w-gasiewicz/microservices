using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.DataServices.Http;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if(builder.Environment.IsDevelopment())
{    
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
    Console.WriteLine("--> using inmem db");
}
else
{
    Console.WriteLine("--> using sqlserver db");
    builder.Services.AddDbContext<AppDbContext>(opt 
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();//use of httpclient factory
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
