


public interface IReservas
{
    Task<(bool Ok, string? Error, Reserva? Reserva)> CrearAsync(Reserva reserva);
    Task<List<Reserva>> ListarPorBarberoAsync(int barberoId, DateTime fecha);
    Task<List<Reserva>> ListarMiasAsync(string telefono);
    Task<bool> CancelarAsync(int id);
    Task<List<DateTime>> HorariosLibresAsync(int barberoId, DateTime fecha);
    Task<Reserva?> ObtenerAsync(int id);
    Task<(bool Ok, string? Error)> ActualizarAsync(Reserva datos);
}