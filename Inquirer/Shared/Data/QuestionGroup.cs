using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inquirer.Data
{
    public sealed class QuestionGroup : Entity<QuestionGroup>, IOrderedEntity
    {
        public static readonly int MaxCount = 10;
        public int Number { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }

        public int QuestionnaireId { get; set; }
        [JsonIgnore]
        public Questionnaire Questionnaire { get; set; }

        public IList<Question> Questions { get; set; }

        public override QuestionGroup ForDatabase() => new ()
        {
            Id = Id,
            Number = Number,
            Name = Name,
            Description = Description,
            Score = Score,
            QuestionnaireId = QuestionnaireId,
        };
    }
}
