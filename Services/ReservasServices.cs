using BarberClub.Data;
using BarberClub.Models;
using Microsoft.EntityFrameworkCore;


public class ReservasService : IReservas
{
    private readonly BarberClubDbContext _db;

    // Horario de atención
    private static readonly TimeSpan Apertura = TimeSpan.FromHours(9);
    private static readonly TimeSpan Cierre = TimeSpan.FromHours(18);
    private static readonly TimeSpan Duracion = TimeSpan.FromHours(1);

    public ReservasService(BarberClubDbContext db) => _db = db;

    public async Task<(bool Ok, string? Error, Reserva? Reserva)> CrearAsync(Reserva reserva)
    {
        if (reserva.Inicio <= DateTime.Now)
            return (false, "La fecha y hora deben ser futuras.", null);

        var barberoOk = await _db.Barberos.AnyAsync(b => b.Id == reserva.BarberoId && b.Activo);
        if (!barberoOk)
            return (false, "El barbero seleccionado no está disponible.", null);

        var servicioOk = await _db.Servicios.AnyAsync(s => s.Id == reserva.ServicioId);
        if (!servicioOk)
            return (false, "El servicio seleccionado no existe.", null);

        // LA REGLA: no repetir barbero y hora
        var ocupado = await _db.Reservas.AnyAsync(r =>
            r.BarberoId == reserva.BarberoId && r.Inicio == reserva.Inicio);
        if (ocupado)
            return (false, "Ese barbero ya tiene una reserva a esa hora. Elige otro horario.", null);

        reserva.Estado = Reserva.EstadoActiva;
        _db.Reservas.Add(reserva);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Si dos personas reservan a la vez, el índice único de la base lo detiene
            return (false, "Ese horario acaba de ser reservado. Elige otro.", null);
        }

        return (true, null, reserva);
    }

    public async Task<List<Reserva>> ListarPorBarberoAsync(int barberoId, DateTime fecha)
    {
        var dia = fecha.Date;
        return await _db.Reservas
            .Include(r => r.Servicio)
            .Where(r => r.BarberoId == barberoId && r.Inicio >= dia && r.Inicio < dia.AddDays(1))
            .OrderBy(r => r.Inicio)
            .ToListAsync();
    }

    public async Task<List<Reserva>> ListarMiasAsync(string telefono)
    {
        return await _db.Reservas
            .Include(r => r.Servicio)
            .Include(r => r.Barbero)
            .Where(r => r.Telefono == telefono)
            .OrderBy(r => r.Inicio)
            .ToListAsync();
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var reserva = await _db.Reservas.FindAsync(id);
        if (reserva is null) return false;

        // Se elimina para que la hora vuelva a quedar libre (por el índice único)
        _db.Reservas.Remove(reserva);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<DateTime>> HorariosLibresAsync(int barberoId, DateTime fecha)
    {
        var dia = fecha.Date;

        // Horas ya ocupadas ese día por ese barbero
        var ocupadas = await _db.Reservas
            .Where(r => r.BarberoId == barberoId && r.Inicio >= dia && r.Inicio < dia.AddDays(1))
            .Select(r => r.Inicio)
            .ToListAsync();

        // Genero los cupos del día y descarto los ocupados o ya pasados
        var libres = new List<DateTime>();
        for (var hora = dia + Apertura; hora + Duracion <= dia + Cierre; hora += Duracion)
        {
            if (hora > DateTime.Now && !ocupadas.Contains(hora))
                libres.Add(hora);
        }
        return libres;
    }
}