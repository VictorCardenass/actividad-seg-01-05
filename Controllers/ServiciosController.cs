using Microsoft.AspNetCore.Mvc;

public class ServiciosController : Controller
{
    private readonly ICatalogo catalogo;

    public ServiciosController(ICatalogo catalogo)
    {
        this.catalogo = catalogo;
    }

    [HttpGet("servicios")]
    public IActionResult Listar()
    {
        return View(catalogo.Listar());
    }
}
