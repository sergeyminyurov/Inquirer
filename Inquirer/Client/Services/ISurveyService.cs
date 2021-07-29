using Inquirer.Data;
using System.Threading.Tasks;

namespace Inquirer.Services
{
    public interface ISurveyService
    {
        Task<SurveyAnswer> SetAnswer(Survey survey, Question question, QuestionAnswer answer, bool single);
        Task RemoveAnswer(Survey survey, QuestionAnswer answer);
    }
}