using System;
using System.Collections.Generic;

namespace Inquirer.Data
{
    public sealed class Survey : Entity<Survey>
    {
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CompleteTime { get; set; }

        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }

        public string RespondentId { get; set; }
        public ApplicationUser Respondent { get; set; }

        public IList<SurveyAnswer> Answers { get; set; }

        public override Survey ForDatabase() => new ()
        {
            Id = Id,
            Comment = Comment,
            CreateTime = CreateTime,
            CompleteTime = CompleteTime,
            QuestionnaireId = QuestionnaireId,
            RespondentId = RespondentId,
        };
    }
}