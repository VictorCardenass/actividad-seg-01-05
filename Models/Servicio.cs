public class Servicio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Duracion { get; set; } // en minutos
    public bool Activo { get; set; } = true;
}
