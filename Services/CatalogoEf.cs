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
}
