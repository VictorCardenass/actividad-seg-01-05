using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Administrador")]
[Route("admin/reservas")]
public class AdminReservasController : Controller
{
    private readonly BarberClubDbContext db;

    public AdminReservasController(BarberClubDbContext db)
    {
        this.db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var reservas = db.Reservas
            .Include(r => r.Servicio)
            .Include(r => r.Barbero)
            .OrderByDescending(r => r.Inicio)
            .ToList();

        return View(reservas);
    }
}
