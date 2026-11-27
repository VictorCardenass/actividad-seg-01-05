using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;


public class ReservaNuevaViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Elige un servicio.")]
    [Display(Name = "Servicio")]
    public int ServicioId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Elige un barbero.")]
    [Display(Name = "Barbero")]
    public int BarberoId { get; set; }

    [Required(ErrorMessage = "Indica la fecha y la hora.")]
    [Display(Name = "Fecha y hora")]
    public DateTime? Inicio { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede pasar de 20 caracteres.")]
    [RegularExpression(@"^[0-9+\s-]{7,20}$", ErrorMessage = "Escribe un teléfono válido (solo números, +, espacios o guiones).")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    // Solo para llenar los desplegables, no se validan
    [ValidateNever] public List<SelectListItem> Servicios { get; set; } = new();
    [ValidateNever] public List<SelectListItem> Barberos { get; set; } = new();
}