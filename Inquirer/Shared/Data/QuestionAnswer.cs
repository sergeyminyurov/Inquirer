using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inquirer.Data
{
    public sealed class QuestionAnswer : Entity<QuestionAnswer>
    {
        public static readonly int MaxCount = 10;
        public int Number { get; set; }
        [Required]
        public string Text { get; set; }
        public decimal Score { get; set; }
        public bool IsCorrect => Score > 0;

        public int QuestionId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }

        public IList<SurveyAnswer> SurveyAnswers { get; set; }

        public override QuestionAnswer ForDatabase() => new ()
        {
            Id = Id,
            Number = Number,
            Text = Text,
            Score = Score,
            QuestionId = QuestionId,
        };
    }
}