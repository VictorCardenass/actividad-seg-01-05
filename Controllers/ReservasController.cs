using BarberClub.Data;
using BarberClub.Models;
using BarberClub.Services;
using BarberClub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarberClub.Controllers;

[Route("reservas")]
public class ReservasController : Controller
{
    private readonly IReservas _reservas;
    private readonly BarberClubDbContext _db;

    public ReservasController(IReservas reservas, BarberClubDbContext db)
    {
        _reservas = reservas;
        _db = db;
    }

    // GET: muestra el formulario vacío
    [HttpGet("nueva")]
    public async Task<IActionResult> Nueva()
    {
        var vm = new ReservaNuevaViewModel();
        await CargarListas(vm);
        return View(vm);
    }

    // POST: recibe lo que envió el usuario
    [HttpPost("nueva")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nueva(ReservaNuevaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await CargarListas(vm);
            return View(vm); // misma vista, con lo que el usuario escribió
        }

        var reserva = new Reserva
        {
            ServicioId = vm.ServicioId,
            BarberoId = vm.BarberoId,
            Inicio = vm.Inicio!.Value,
            NombreCliente = vm.NombreCliente.Trim(),
            Telefono = vm.Telefono.Trim()
        };

        var (ok, error, _) = await _reservas.CrearAsync(reserva);
        if (!ok)
        {
            ModelState.AddModelError(string.Empty, error!);
            await CargarListas(vm);
            return View(vm);
        }

        return RedirectToAction(nameof(Mias), new { telefono = vm.Telefono.Trim() });
    }

    // La vista de esta acción la hacemos en el punto 5
    [HttpGet("mias")]
    public async Task<IActionResult> Mias(string? telefono)
    {
        ViewBag.Telefono = telefono;
        var lista = string.IsNullOrWhiteSpace(telefono)
            ? new List<Reserva>()
            : await _reservas.ListarMiasAsync(telefono);
        return View(lista);
    }

    // Llena los desplegables de servicio y barbero
    private async Task CargarListas(ReservaNuevaViewModel vm)
    {
        vm.Servicios = await _db.Servicios
            .OrderBy(s => s.Nombre)
            .Select(s => new SelectListItem(s.Nombre, s.Id.ToString()))
            .ToListAsync();

        vm.Barberos = await _db.Barberos
            .Where(b => b.Activo)
            .OrderBy(b => b.Nombre)
            .Select(b => new SelectListItem(b.Nombre, b.Id.ToString()))
            .ToListAsync();
    }
}