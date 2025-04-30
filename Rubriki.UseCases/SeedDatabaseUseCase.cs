using Rubriki.Cqrs;

namespace Rubriki.UseCases;

public class SeedDatabaseUseCase(AdminCommand adminCommand)
{
    public async Task Invoke()
    {
        await adminCommand.ClearData();
        await adminCommand.SeedData(seedData);
    }

    private static readonly Dto.SeedData seedData = new Dto.SeedData
    {
        CategoryAndCriteria =
        [
            new("Robot Design", ["Mechanical", "Programming", "Innovation" ]),
            new("Project", [ "Research", "Solution", "Presentation" ]),
            new("Core Values", [ "Inspiration", "Teamwork", "Professionalism" ]),
        ],
        ContestantNames = 
        [
            "Astro Bots",
            "Bot Heads", 
            "Code Commets",
            "Galactic Gearheads", 
            "Hydro Hackers",
            "Mech Masters", 
            "Null Terminators", 
            "Robo Rangers", 
            "Salty Circuits",
            "Wattage Warriors",
        ],
        JudgeNames = 
        [
            "Alice Anderson", 
            "Brian Bennett", 
            "Catherine Carter", 
            "Daniel Davis"
        ],
        Levels =
        [
            "Beginner",
            "Developing",
            "Accomplished",
            "Exemplary"
        ]
    };
}
