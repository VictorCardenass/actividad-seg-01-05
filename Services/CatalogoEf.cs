public class CatalogoEf : ICatalogo
{
    private readonly BarberClubDbContext db;

    public CatalogoEf(BarberClubDbContext db)
    {
        this.db = db;
    }

    public List<Servicio> Listar()
    {
        return db.Servicios.Where(s => s.Activo).OrderBy(s => s.Nombre).ToList();
    }

    public Servicio? Buscar(int id)
    {
        return db.Servicios.Find(id);
    }

    public List<Servicio> ListarTodos()
    {
        return db.Servicios.OrderBy(s => s.Nombre).ToList();
    }

    public void Crear(Servicio servicio)
    {
        servicio.Activo = true;
        db.Servicios.Add(servicio);
        db.SaveChanges();
    }

    public bool Editar(Servicio servicio)
    {
        var existente = db.Servicios.Find(servicio.Id);

        if (existente is null)
        {
            return false;
        }

        existente.Nombre = servicio.Nombre;
        existente.Precio = servicio.Precio;
        existente.Duracion = servicio.Duracion;
        db.SaveChanges();
        return true;
    }

    public bool CambiarEstado(int id, bool activo)
    {
        var existente = db.Servicios.Find(id);

        if (existente is null)
        {
            return false;
        }

        existente.Activo = activo;
        db.SaveChanges();
        return true;
    }
}
