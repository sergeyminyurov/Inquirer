using Inquirer.Data;

namespace Inquirer.Controllers
{
    public sealed class QuestionAnswerController : EntityController<QuestionAnswer, int>
    {
        public QuestionAnswerController(InquirerDbContext db) : base(db) { }
    }
}