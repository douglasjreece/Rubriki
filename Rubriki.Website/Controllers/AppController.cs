using Microsoft.AspNetCore.Mvc;
using Rubriki.Dto;

namespace Rubriki.Website.Controllers;

[ApiController]
[Route("/api/app")]
public class AppController(IServiceProvider provider) : ControllerBase
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
        [FromBody] ScoreSubmission? submission)
    {
        if (submission is null)
        {
            return BadRequest("Score data is required");
        }
        var command = provider.GetRequiredService<Cqrs.ClientCommand>();
        await command.SetScore(contestantId, submission.JudgeId, submission.CriteriaId, submission.Score, submission.Comment);
        return Ok();
    }
}
