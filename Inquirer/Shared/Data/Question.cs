using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inquirer.Data
{
    public sealed class Question : Entity<Question>
    {
        public static readonly int MaxCount = 10;
        public Question()
        {
            Answers = new List<QuestionAnswer>();
        }
        public int Number { get; set; }
        [Required]
        public string Text { get; set; }

        public int QuestionnaireId { get; set; }
        [JsonIgnore]
        public Questionnaire Questionnaire { get; set; }

        public int? GroupId { get; set; }
        [JsonIgnore]
        public QuestionGroup Group { get; set; }

        public IList<QuestionAnswer> Answers { get; set; }       

        public override Question ForDatabase() => new () 
        {
            Id = Id,
            Number = Number,
            Text = Text,
            QuestionnaireId = QuestionnaireId,
            GroupId = GroupId
        };
    }
}