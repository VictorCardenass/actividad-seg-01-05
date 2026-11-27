using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/reservas")]
public class ReservasApiController : ControllerBase
{
    private readonly IReservas reservas;
    private readonly BarberClubDbContext db;

    public ReservasApiController(IReservas reservas, BarberClubDbContext db)
    {
        this.reservas = reservas;
        this.db = db;
    }

    [HttpGet("~/api/barberos")]
    [ProducesResponseType(typeof(List<BarberoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BarberoDto>>> Barberos()
    {
        return await db.Barberos
            .Where(b => b.Activo)
            .OrderBy(b => b.Nombre)
            .Select(b => new BarberoDto { Id = b.Id, Nombre = b.Nombre })
            .ToListAsync();
    }

    [HttpGet("horarios-libres")]
    [ProducesResponseType(typeof(List<DateTime>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<DateTime>>> HorariosLibres(int barberoId, DateTime? fecha)
    {
        if (barberoId <= 0 || fecha is null)
        {
            return BadRequest(new { error = "Indica barberoId y fecha (por ejemplo 2026-11-30)." });
        }

        return await reservas.HorariosLibresAsync(barberoId, fecha.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservaRespuestaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear(ReservaCrearDto datos)
    {
        var inicio = datos.Inicio!.Value;

        var barberoOk = await db.Barberos.AnyAsync(b => b.Id == datos.BarberoId && b.Activo);
        if (!barberoOk)
        {
            return BadRequest(new { error = "El barbero no existe o no está disponible." });
        }

        var servicioOk = await db.Servicios.AnyAsync(s => s.Id == datos.ServicioId && s.Activo);
        if (!servicioOk)
        {
            return BadRequest(new { error = "El servicio no existe o no está disponible." });
        }

        var libres = await reservas.HorariosLibresAsync(datos.BarberoId, inicio);
        if (!libres.Contains(inicio))
        {
            return Conflict(new { error = "Ese horario no está disponible. Elige uno de los horarios libres." });
        }

        var reserva = new Reserva
        {
            ServicioId = datos.ServicioId,
            BarberoId = datos.BarberoId,
            Inicio = inicio,
            NombreCliente = datos.NombreCliente.Trim(),
            Telefono = datos.Telefono.Trim()
        };

        var (ok, error, creada) = await reservas.CrearAsync(reserva);
        if (!ok)
        {
            return Conflict(new { error });
        }

        return StatusCode(StatusCodes.Status201Created, new ReservaRespuestaDto
        {
            Id = creada!.Id,
            ServicioId = creada.ServicioId,
            BarberoId = creada.BarberoId,
            Inicio = creada.Inicio,
            NombreCliente = creada.NombreCliente,
            Telefono = creada.Telefono,
            Estado = creada.Estado
        });
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar(int id, string? telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
        {
            return BadRequest(new { error = "Indica el teléfono con el que se hizo la reserva." });
        }

        var reserva = await reservas.ObtenerAsync(id);
        if (reserva is null || reserva.Telefono != telefono.Trim())
        {
            return NotFound(new { error = "No encontramos una reserva con ese número y teléfono." });
        }

        await reservas.CancelarAsync(id);
        return NoContent();
    }
}
