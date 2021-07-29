using Inquirer.Data;
using Inquirer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inquirer.Controllers
{
    public sealed class QuestionGroupController : EntityController<QuestionGroup, int>
    {
        public QuestionGroupController(InquirerDbContext db) : base(db) { }

        [HttpPut("swap")]
        public void Swap(SwapRequestData<QuestionGroup> data)
        {
            DbContext.SwapOrdered(data.Entity1, data.Entity2);
        }
    }
}