using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AgendaViewModel
{
    public int? BarberoId { get; set; }

    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    public List<SelectListItem> Barberos { get; set; } = new();
    public List<Reserva> Reservas { get; set; } = new();
}