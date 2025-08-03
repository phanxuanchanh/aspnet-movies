using Microsoft.AspNetCore.Mvc;
using MovieCDN.AppCodes;
using MovieCDN.Database;

namespace MovieCDN.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly MovieCdnContext _context;

    public AuthController(TokenService tokenService, MovieCdnContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult> Authenticate([FromBody]ApiKey apiKey)
    {
        ApiKey? apiKeyFromDb = await _context.ApiKeys.FindAsync(apiKey.ClientId);
        if (apiKeyFromDb is null)
            return NotFound();

        if(apiKeyFromDb.SecretKey != apiKey.SecretKey)
            return Unauthorized("Invalid API key or secret key");

        string token = _tokenService.GenerateToken(apiKeyFromDb.ClientId);
        return Ok(new { token });
    }
}
