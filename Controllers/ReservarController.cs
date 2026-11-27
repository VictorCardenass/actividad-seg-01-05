using Microsoft.AspNetCore.Mvc;

public class ReservarController : Controller
{
    [HttpGet("reservar")]
    public IActionResult Index()
    {
        return View();
    }
}
