using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrador")]
[Route("admin/servicios")]
public class AdminServiciosController : Controller
{
    private readonly ICatalogo catalogo;

    public AdminServiciosController(ICatalogo catalogo)
    {
        this.catalogo = catalogo;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View(catalogo.ListarTodos());
    }

    [HttpGet("crear")]
    public IActionResult Crear()
    {
        return View(new ServicioFormulario());
    }

    [HttpPost("crear")]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(ServicioFormulario modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        catalogo.Crear(new Servicio
        {
            Nombre = modelo.Nombre.Trim(),
            Precio = modelo.Precio,
            Duracion = modelo.Duracion
        });

        return Redirect("/admin/servicios");
    }

    [HttpGet("editar/{id:int}")]
    public IActionResult Editar(int id)
    {
        var servicio = catalogo.Buscar(id);

        if (servicio is null)
        {
            return NotFound();
        }

        return View(new ServicioFormulario
        {
            Id = servicio.Id,
            Nombre = servicio.Nombre,
            Precio = servicio.Precio,
            Duracion = servicio.Duracion
        });
    }

    [HttpPost("editar/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, ServicioFormulario modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var cambiado = catalogo.Editar(new Servicio
        {
            Id = id,
            Nombre = modelo.Nombre.Trim(),
            Precio = modelo.Precio,
            Duracion = modelo.Duracion
        });

        if (!cambiado)
        {
            return NotFound();
        }

        return Redirect("/admin/servicios");
    }

    [HttpPost("estado/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult CambiarEstado(int id, bool activo)
    {
        if (!catalogo.CambiarEstado(id, activo))
        {
            return NotFound();
        }

        return Redirect("/admin/servicios");
    }
}
