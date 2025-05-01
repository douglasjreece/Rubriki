using Microsoft.AspNetCore.Mvc;
using Rubriki.Dto;
using Rubriki.Authentication;

namespace Rubriki.Website.Controllers;

[ApiController]
[Route("/api/app")]
public class AppController(IServiceProvider provider, ISecretCodeAuthenticationService authenticationService) : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult GetHello()
    {
        return Ok("Hello from Rubriki API");
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
    {
        AuthenticationResult authState = await authenticationService.SignIn(request.SecretCode, persist: false);
        if (!authState.IsAuthenticated)
        {
            return Unauthorized("Invalid secret code");
        }
        return Ok(authState);
    }

    [HttpGet("seed-data")]
    public async Task<IActionResult> GetSeedData([FromHeader] string token)
    {
        var authState = await authenticationService.GetStateForToken(token);
        if (!authState.IsAuthenticated)
        {
            return Unauthorized("Invalid token");
        }
        if (authState.Role != RoleName.Admin)
        {
            return Unauthorized("Only admins can access seed data");
        }

        var seedData = provider.GetService<SeedData>();
        return Ok(seedData);
    }

    [HttpPut("submit-score/{contestantId}")]
    public async Task<IActionResult> SubmitScore(
        int contestantId,
        [FromBody] ScoreSubmission? submission,
        [FromHeader] string token)
    {
        var authState = await authenticationService.GetStateForToken(token);
        if (!authState.IsAuthenticated)
        {
            return Unauthorized("Invalid token");
        }

        if (submission is null)
        {
            return BadRequest("Score data is required");
        }
        var command = provider.GetRequiredService<Cqrs.ClientCommand>();
        await command.SetScore(contestantId, submission.JudgeId, submission.CriteriaId, submission.LevelId, submission.Comment);
        return Ok();
    }
}
