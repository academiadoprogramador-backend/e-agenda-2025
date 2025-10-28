using eAgenda.Dominio.ModuloAutenticacao;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Controllers;

[Route("autenticacao")]
public class AutenticacaoController(
    SignInManager<Usuario> signInManager,
    UserManager<Usuario> userManager
) : Controller
{
    [HttpGet("registro")]
    public IActionResult Registro()
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction("Index", "Home");

        var registroVm = new RegistroViewModel();

        return View(registroVm);
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroViewModel registroVm)
    {
        var usuario = new Usuario()
        {
            UserName = registroVm.Email,
            Email = registroVm.Email,
        };

        var usuarioResult = await userManager.CreateAsync(usuario, registroVm.Senha);

        if (!usuarioResult.Succeeded)
        {
            var erros = usuarioResult.Errors.Select(err =>
            {
                return err.Code switch
                {
                    "DuplicateUserName" => "Já existe um usuário com esse nome.",
                    "DuplicateEmail" => "Já existe um usuário com esse e-mail.",
                    "PasswordTooShort" => "A senha é muito curta.",
                    "PasswordRequiresNonAlphanumeric" => "A senha deve conter pelo menos um caractere especial.",
                    "PasswordRequiresDigit" => "A senha deve conter pelo menos um número.",
                    "PasswordRequiresUpper" => "A senha deve conter pelo menos uma letra maiúscula.",
                    "PasswordRequiresLower" => "A senha deve conter pelo menos uma letra minúscula.",
                    _ => err.Description
                };
            }).ToList();

            ModelState.AddModelError("ErroAutenticacao", erros.First());

            return View(registroVm);
        }

        await signInManager.PasswordSignInAsync(
            usuario.Email,
            registroVm.Senha,
            isPersistent: true,
            lockoutOnFailure: false
        );

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        var loginVm = new LoginViewModel();

        ViewData["ReturnUrl"] = returnUrl;

        return View(loginVm);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginVm, string? returnUrl = null)
    {
        var resultadoLogin = await signInManager.PasswordSignInAsync(
              loginVm.Email,
              loginVm.Senha,
              isPersistent: true,
              lockoutOnFailure: false
          );

        if (!resultadoLogin.Succeeded)
        {
            ModelState.AddModelError("ErroAutenticacao", "Login ou senha incorretos.");

            return View(loginVm);
        }

        if (Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }
}
