using System.ComponentModel.DataAnnotations;

public class ServicioFormulario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe el nombre del servicio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede pasar de 60 caracteres.")]
    public string Nombre { get; set; } = "";

    [Range(1000, 1000000, ErrorMessage = "El precio debe estar entre 1.000 y 1.000.000.")]
    public decimal Precio { get; set; }

    [Range(5, 240, ErrorMessage = "La duración debe estar entre 5 y 240 minutos.")]
    public int Duracion { get; set; }
}
