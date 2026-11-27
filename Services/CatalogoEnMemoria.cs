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

    public List<Servicio> ListarTodos()
    {
        return servicios.OrderBy(s => s.Nombre).ToList();
    }

    public void Crear(Servicio servicio)
    {
        servicio.Id = servicios.Count == 0 ? 1 : servicios.Max(s => s.Id) + 1;
        servicio.Activo = true;
        servicios.Add(servicio);
    }

    public bool Editar(Servicio servicio)
    {
        var existente = servicios.FirstOrDefault(s => s.Id == servicio.Id);

        if (existente is null)
        {
            return false;
        }

        existente.Nombre = servicio.Nombre;
        existente.Precio = servicio.Precio;
        existente.Duracion = servicio.Duracion;
        return true;
    }

    public bool CambiarEstado(int id, bool activo)
    {
        var existente = servicios.FirstOrDefault(s => s.Id == id);

        if (existente is null)
        {
            return false;
        }

        existente.Activo = activo;
        return true;
    }
}
