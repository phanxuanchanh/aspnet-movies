using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieCDN.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [Route("")]
    [HttpGet]
    public IActionResult Authenticate()
    {
        string token = _tokenService.GenerateToken("mainClient");
        return Ok(new { token });
    }
}
