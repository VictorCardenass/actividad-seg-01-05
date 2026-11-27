using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BarberClubDbContext>(opciones =>
    opciones.UseNpgsql(builder.Configuration.GetConnectionString("BarberClub")));
builder.Services.AddScoped<ICatalogo, CatalogoEf>();
builder.Services.AddScoped<IReservas, ReservasService>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opciones =>
    {
        opciones.LoginPath = "/login";
        opciones.AccessDeniedPath = "/acceso-denegado";
    });

var app = builder.Build();

using (var alcance = app.Services.CreateScope())
{
    try
    {
        var db = alcance.ServiceProvider.GetRequiredService<BarberClubDbContext>();
        var hasher = alcance.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();
        Sembrador.Sembrar(db, hasher);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "No se pudieron crear los usuarios de ejemplo.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseWhen(
    contexto => !contexto.Request.Path.StartsWithSegments("/api"),
    ramaWeb => ramaWeb.UseStatusCodePagesWithReExecute("/error/{0}"));

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapControllers();

app.Run();
