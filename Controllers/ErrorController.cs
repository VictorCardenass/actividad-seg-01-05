using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("error/{codigo:int?}")]
    public IActionResult Index(int? codigo)
    {
        if (codigo == 404)
        {
            Response.StatusCode = 404;
            return View("NoEncontrado");
        }

        Response.StatusCode = codigo ?? 500;
        return View("Fallo");
    }
}
