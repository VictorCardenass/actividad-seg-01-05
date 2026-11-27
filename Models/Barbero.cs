using System.ComponentModel.DataAnnotations;


public class Barbero
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    // Un barbero tiene muchas reservas
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}