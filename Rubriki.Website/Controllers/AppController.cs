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

    [HttpGet("seed-data")]
    public IActionResult GetSeedData()
    {
        var seedData = provider.GetService<SeedData>();
        return Ok(seedData);
    }

    [HttpPut("submit-score/{contestantId}")]
    public async Task<IActionResult> SubmitScore(
        int contestantId,
        [FromBody] ScoreSubmission? submission,
        [FromHeader] string secretCode)
    {
        var authState = await authenticationService.SignIn(secretCode, persist:false);
        if (!authState.IsAuthenticated)
        {
            return Unauthorized("Invalid secret code");
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
