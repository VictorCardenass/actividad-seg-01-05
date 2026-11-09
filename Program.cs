var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ICatalogo, CatalogoEnMemoria>();

var app = builder.Build();

app.MapControllers();

app.Run();
