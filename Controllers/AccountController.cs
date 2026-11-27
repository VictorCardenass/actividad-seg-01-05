using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly BarberClubDbContext db;
    private readonly IPasswordHasher<Usuario> hasher;

    public AccountController(BarberClubDbContext db, IPasswordHasher<Usuario> hasher)
    {
        this.db = db;
        this.hasher = hasher;
    }

    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var correo = modelo.Correo.Trim().ToLowerInvariant();
        var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo);

        var resultado = usuario is null
            ? PasswordVerificationResult.Failed
            : hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, modelo.Password);

        if (usuario is null || resultado == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View(modelo);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString())
        };
        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidad));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return Redirect("/servicios");
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }

    [HttpGet("acceso-denegado")]
    public IActionResult AccesoDenegado()
    {
        Response.StatusCode = 403;
        return View();
    }
}
