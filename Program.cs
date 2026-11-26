using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BarberClubDbContext>(opciones =>
    opciones.UseNpgsql(builder.Configuration.GetConnectionString("BarberClub")));
builder.Services.AddScoped<ICatalogo, CatalogoEf>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseWhen(
    contexto => !contexto.Request.Path.StartsWithSegments("/api"),
    ramaWeb => ramaWeb.UseStatusCodePagesWithReExecute("/error/{0}"));

app.MapOpenApi();
app.MapControllers();

app.Run();
