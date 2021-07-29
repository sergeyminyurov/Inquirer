using Inquirer.Data;

namespace Inquirer.Controllers
{
    public sealed class QuestionController : EntityController<Question, int>
    {
        public QuestionController(InquirerDbContext db) : base(db) { }
    }
}