using System.ComponentModel.DataAnnotations;

public class BarberoFormulario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe el nombre del barbero.")]
    [StringLength(80, ErrorMessage = "El nombre no puede pasar de 80 caracteres.")]
    public string Nombre { get; set; } = "";

    public bool Activo { get; set; } = true;
}
