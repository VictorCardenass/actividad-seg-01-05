public class CatalogoEnMemoria : ICatalogo
{
    private readonly List<Servicio> servicios = new()
    {
        new Servicio { Id = 1, Nombre = "Corte clásico", Precio = 25000m, Duracion = 30 },
        new Servicio { Id = 2, Nombre = "Afeitado premium", Precio = 20000m, Duracion = 20 },
        new Servicio { Id = 3, Nombre = "Masaje facial", Precio = 35000m, Duracion = 25 },
    };

    public Servicio? Buscar(int id)
    {
        return servicios.FirstOrDefault(s => s.Id == id);
    }

    public List<Servicio> Listar()
    {
        return servicios.Where(s => s.Activo).ToList();
    }
}
