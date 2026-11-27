using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrador")]
[Route("admin/barberos")]
public class AdminBarberosController : Controller
{
    private readonly BarberClubDbContext db;

    public AdminBarberosController(BarberClubDbContext db)
    {
        this.db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View(db.Barberos.OrderBy(b => b.Nombre).ToList());
    }

    [HttpGet("crear")]
    public IActionResult Crear()
    {
        return View(new BarberoFormulario());
    }

    [HttpPost("crear")]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(BarberoFormulario modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        db.Barberos.Add(new Barbero { Nombre = modelo.Nombre.Trim(), Activo = modelo.Activo });
        db.SaveChanges();
        return Redirect("/admin/barberos");
    }

    [HttpGet("editar/{id:int}")]
    public IActionResult Editar(int id)
    {
        var barbero = db.Barberos.Find(id);

        if (barbero is null)
        {
            return NotFound();
        }

        return View(new BarberoFormulario { Id = barbero.Id, Nombre = barbero.Nombre, Activo = barbero.Activo });
    }

    [HttpPost("editar/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, BarberoFormulario modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var barbero = db.Barberos.Find(id);

        if (barbero is null)
        {
            return NotFound();
        }

        barbero.Nombre = modelo.Nombre.Trim();
        barbero.Activo = modelo.Activo;
        db.SaveChanges();
        return Redirect("/admin/barberos");
    }
}
