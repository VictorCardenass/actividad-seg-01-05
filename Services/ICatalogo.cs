public interface ICatalogo
{
    List<Servicio> Listar();

    Servicio? Buscar(int id);
}
