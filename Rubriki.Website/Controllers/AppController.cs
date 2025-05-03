using Microsoft.AspNetCore.Mvc;
using Rubriki.Dto;
using Rubriki.Authentication;
using Rubriki.Api;
using Rubriki.UseCases;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Rubriki.Website.Controllers;

[ApiController]
[Route(ApiConst.AppPath)]
public class AppController(IServiceProvider provider, ISecretCodeAuthenticationService authenticationService) : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult GetHello()
    {
        return Ok("Hello from Rubriki API");
    }

    [HttpPost(ApiConst.AuthenticationResource)]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
    {
        AuthenticationResult authState = await authenticationService.SignIn(request.SecretCode, persist: false);
        if (!authState.IsAuthenticated)
        {
            return Unauthorized("Invalid secret code");
        }
        return Ok(authState);
    }

    [HttpGet(ApiConst.SeedDataResource)]
    public async Task<IActionResult> GetSeedData([FromHeader(Name = ApiConst.TokenHeader)] string token)
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

        var useCase = provider.GetRequiredService<SeedDatabaseUseCase>();
        var seedData = await useCase.GetCurrentSeedData() ?? throw new InvalidOperationException("Seed data not found.");
        return Ok(seedData);
    }

    [HttpPut($"{ApiConst.ScoreResource}/{{contestantId}}")]
    public async Task<IActionResult> SubmitScore(
        int contestantId,
        [FromBody] ScoreSubmission? submission,
        [FromHeader(Name = ApiConst.TokenHeader)] string token)
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
        var command = provider.GetRequiredService<Cqrs.ScoreCommand>();
        await command.SetScore(contestantId, submission.JudgeId, submission.CriteriaId, submission.LevelId, submission.Comment);
        return Ok();
    }
}
