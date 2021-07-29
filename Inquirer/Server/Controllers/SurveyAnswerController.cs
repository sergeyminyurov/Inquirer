using Inquirer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Inquirer.Controllers
{
    public sealed class SurveyAnswerController : EntityController<SurveyAnswer, int>
    {
        public SurveyAnswerController(InquirerDbContext db) : base(db) { }

        [HttpPost("{surveyId}/{questionId}/{answerId}/{single}")]
        public async Task<SurveyAnswer> SetAnswer(int surveyId, int questionId, int answerId, bool single)
        {
            DbContext.Database.BeginTransaction();
            if (single)
            {
                var prevAnswer = DbContext
                    .SurveyAnswers
                    .Include(t => t.QuestionAnswer)
                    .Where(t => t.SurveyId == surveyId && t.QuestionAnswer.QuestionId == questionId)
                    .FirstOrDefault();
                if (prevAnswer is not null)
                {
                    DbContext.SurveyAnswers.Remove(prevAnswer);
                }
            }
            var answer = new SurveyAnswer
            {
                SurveyId = surveyId,
                QuestionAnswerId = answerId,
            };
            DbContext.SurveyAnswers.Add(answer);
            await DbContext.SaveChangesAsync();
            await DbContext.Database.CommitTransactionAsync();
            return answer;
        }

        [HttpDelete("{surveyId}/{answerId}")]
        public async Task<bool> RemoveAnswer(int surveyId, int answerId)
        {
            var answer = await DbContext.SurveyAnswers.FirstOrDefaultAsync(t => t.SurveyId == surveyId && t.QuestionAnswerId == answerId);
            if (answer is not null)
            {
                DbContext.SurveyAnswers.Remove(answer);
                await DbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}