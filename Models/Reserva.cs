using System.ComponentModel.DataAnnotations;

namespace BarberClub.Models;

public class Reserva
{
    public const string EstadoActiva = "Activa";
    public const string EstadoCancelada = "Cancelada";

    public int Id { get; set; }

    // Relación con Servicio: ServicioId es la columna, Servicio es el objeto
    public int ServicioId { get; set; }
    public Servicio? Servicio { get; set; }

    // Relación con Barbero
    public int BarberoId { get; set; }
    public Barbero? Barbero { get; set; }

    public DateTime Inicio { get; set; }

    [Required, StringLength(100)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(20)]
    public string Estado { get; set; } = EstadoActiva;
}