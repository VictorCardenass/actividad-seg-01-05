using Microsoft.AspNetCore.Authorization;
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
        vm.Servicios = await _db.Servicios.Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .Select(s => new SelectListItem(s.Nombre, s.Id.ToString()))
            .ToListAsync();

        vm.Barberos = await _db.Barberos
            .Where(b => b.Activo)
            .OrderBy(b => b.Nombre)
            .Select(b => new SelectListItem(b.Nombre, b.Id.ToString()))
            .ToListAsync();
    }

    [Authorize(Roles = "Barbero,Administrador")]
    [HttpGet("agenda")]
public async Task<IActionResult> Agenda(int? barberoId, DateTime? fecha)
{
    var vm = new AgendaViewModel
    {
        BarberoId = barberoId,
        Fecha = (fecha ?? DateTime.Today).Date
    };

    vm.Barberos = await _db.Barberos
        .Where(b => b.Activo)
        .OrderBy(b => b.Nombre)
        .Select(b => new SelectListItem(b.Nombre, b.Id.ToString()))
        .ToListAsync();

    if (barberoId.HasValue)
        vm.Reservas = await _reservas.ListarPorBarberoAsync(barberoId.Value, vm.Fecha);

    return View(vm);
}

[HttpGet("editar/{id:int}")]
public async Task<IActionResult> Editar(int id)
{
    var r = await _reservas.ObtenerAsync(id);
    if (r is null) return NotFound();

    var vm = new ReservaEditarViewModel
    {
        Id = r.Id,
        ServicioId = r.ServicioId,
        BarberoId = r.BarberoId,
        Inicio = r.Inicio,
        NombreCliente = r.NombreCliente,
        Telefono = r.Telefono
    };
    await CargarListas(vm);
    return View(vm);
}

[HttpPost("editar/{id:int}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Editar(int id, ReservaEditarViewModel vm)
{
    if (id != vm.Id) return BadRequest();

    if (!ModelState.IsValid)
    {
        await CargarListas(vm);
        return View(vm); // conserva lo que escribió el usuario
    }

    var datos = new Reserva
    {
        Id = id,
        ServicioId = vm.ServicioId,
        BarberoId = vm.BarberoId,
        Inicio = vm.Inicio!.Value,
        NombreCliente = vm.NombreCliente.Trim(),
        Telefono = vm.Telefono.Trim()
    };

    var (ok, error) = await _reservas.ActualizarAsync(datos);
    if (!ok)
    {
        ModelState.AddModelError(string.Empty, error!);
        await CargarListas(vm);
        return View(vm);
    }

    return RedirectToAction(nameof(Mias), new { telefono = vm.Telefono.Trim() });
}

[HttpPost("cancelar/{id:int}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Cancelar(int id)
{
    var r = await _reservas.ObtenerAsync(id);
    if (r is null) return NotFound();

    var telefono = r.Telefono;
    await _reservas.CancelarAsync(id);
    return RedirectToAction(nameof(Mias), new { telefono });
}
}