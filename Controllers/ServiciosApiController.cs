using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/servicios")]
public class ServiciosApiController : ControllerBase
{
    private readonly ICatalogo catalogo;

    public ServiciosApiController(ICatalogo catalogo)
    {
        this.catalogo = catalogo;
    }

    [HttpGet]
    public ActionResult<List<Servicio>> Listar()
    {
        return catalogo.Listar();
    }

    [HttpGet("{id:int}")]
    public ActionResult<Servicio> Obtener(int id)
    {
        var servicio = catalogo.Buscar(id);

        if (servicio is null || !servicio.Activo)
        {
            return NotFound();
        }

        return servicio;
    }
}
