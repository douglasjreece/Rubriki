using Rubriki.Dto;
using Rubriki.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rubriki.SharedComponents;

public partial class ScoreEntryPanel
{
    public class Model(ISetupQuery setupQuery, IScoreQuery scoreQuery, IScoreCommand command) : IValidatableObject
    {
        public List<Judge> Judges { get; set; } = [];
        
        private string selectedJudgeId = string.Empty;
        public string SelectedJudgeId
        {
            get => selectedJudgeId;
            set
            {
                if (value != selectedJudgeId)
                {
                    selectedJudgeId = value;
                    ChangeMade();
                }
            }
        }

        public List<Category> Categories { get; set; } = [];

        string selectedCategoryId = string.Empty;
        public string SelectedCategoryId 
        { 
            get => selectedCategoryId; 
            set
            {
                if (value != selectedCategoryId)
                {
                    selectedCategoryId = value;
                    ChangeMade();
                    LoadCriteria().ConfigureAwait(false);
                    LoadContestant().ConfigureAwait(false);
                }
            }
        }

        public List<Contestant> Contestants { get; set; } = [];

        string selectedContestantId = string.Empty;
        public string SelectedContestantId
        {
            get => selectedContestantId;
            set
            {
                if (value != selectedContestantId)
                {
                    selectedContestantId = value;
                    ChangeMade();
                    LoadContestant().ConfigureAwait(false);
                }
            }
        }

        public List<Level> Levels { get; set; } = [];

        public List<CriteriaEntry> CriteriaEntries { get; set; } = [];

        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        private List<CriteriaScore> currentScores = [];

        public async Task Load()
        {
            Judges = await setupQuery.GetJudges();
            Categories = await setupQuery.GetCategories();
            Contestants = await setupQuery.GetContestants();
            Levels = await setupQuery.GetLevels();
        }

        public async Task LoadCriteria()
        {
            if (string.IsNullOrEmpty(selectedCategoryId))
            {
                CriteriaEntries = [];
                return;
            }

            if (int.TryParse(selectedCategoryId, out var categoryId))
            {
                var criteria = await setupQuery.GetCriteria(categoryId);
                CriteriaEntries = 
                [
                    .. criteria.Select(x => new CriteriaEntry
                    {
                        Id = x.Id,
                        Name = x.Name,
                        OnChange = ChangeMade
                    })
                ];
            }

            UpdateScores();
        }

        public async Task LoadContestant()
        {
            if (int.TryParse(selectedContestantId, out var contestantId) && int.TryParse(selectedCategoryId, out var categoryId))
            {
                currentScores = await scoreQuery.GetContestantCategoryScores(contestantId, categoryId);
            }
            else
            {
                currentScores = [];
            }
            UpdateScores();
        }


        public async Task SaveScores()
        {
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;

            try
            {
                if (int.TryParse(SelectedContestantId, out var contestantId) && int.TryParse(SelectedJudgeId, out var judgeId))
                {
                    foreach (var entry in CriteriaEntries)
                    {
                        if (int.TryParse(entry.Level, out var score))
                        {
                            await command.SetScore(contestantId, judgeId, entry.Id, score, entry.Comment);
                        }
                    }
                }
                SuccessMessage = "The scores had been saved.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        private void ClearScores()
        {
            foreach (var entry in CriteriaEntries)
            {
                entry.Level = string.Empty;
                entry.Comment = string.Empty;
            }
        }

        private void UpdateScores()
        {
            ClearScores();
            foreach (var currentScore in currentScores)
            {
                var entry = CriteriaEntries.FirstOrDefault(x => x.Id == currentScore.Criteria.Id);
                if (entry != null)
                {
                    entry.Level = currentScore.Level.Id.ToString();
                    entry.Comment = currentScore.Comment;
                }
            }
        }

        private void ChangeMade()
        {
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!int.TryParse(SelectedJudgeId, out _))
            {
                yield return new ValidationResult("Please select a judge.");
            }

            if (!int.TryParse(SelectedCategoryId, out _))
            {
                yield return new ValidationResult("Please select a category.");
            }

            if (!int.TryParse(SelectedContestantId, out _))
            {
                yield return new ValidationResult("Please select a contestant.");
            }

            if (CriteriaEntries.Any(x => !int.TryParse(x.Level, out _)))
            {
                yield return new ValidationResult("Please enter scores for all criteria.");
            }
        }

        public class CriteriaEntry
        {
            public Action? OnChange { get; set; }

            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;

            private string comment = string.Empty;
            public string Comment
            {
                get => comment;
                set
                {
                    if (value != comment)
                    {
                        comment = value;
                        OnChange?.Invoke();
                    }
                }
            }

            private string level = string.Empty;
            public string Level
            {
                get => level;
                set
                {
                    if (value != level)
                    {
                        level = value;
                        OnChange?.Invoke();
                    }
                }
            }
        }
    }
}
