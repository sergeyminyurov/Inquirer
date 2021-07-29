namespace Inquirer.Data
{
    public sealed class SurveyAnswer : Entity<SurveyAnswer>
    {
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public int QuestionAnswerId { get; set; }
        public QuestionAnswer QuestionAnswer { get; set; }

        public override SurveyAnswer ForDatabase() => new () 
        { 
            Id = Id,
            SurveyId = SurveyId,
            QuestionAnswerId = QuestionAnswerId,
        };
    }
}