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
    [ProducesResponseType(typeof(List<Servicio>), StatusCodes.Status200OK)]
    public ActionResult<List<Servicio>> Listar()
    {
        return catalogo.Listar();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Servicio), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
