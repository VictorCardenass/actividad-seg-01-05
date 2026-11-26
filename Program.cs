using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BarberClubDbContext>(opciones =>
    opciones.UseNpgsql(builder.Configuration.GetConnectionString("BarberClub")));
builder.Services.AddScoped<ICatalogo, CatalogoEf>();

var app = builder.Build();

app.MapControllers();

app.Run();
