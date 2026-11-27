public interface ICatalogo
{
    List<Servicio> Listar();

    Servicio? Buscar(int id);

    List<Servicio> ListarTodos();

    void Crear(Servicio servicio);

    bool Editar(Servicio servicio);

    bool CambiarEstado(int id, bool activo);
}
