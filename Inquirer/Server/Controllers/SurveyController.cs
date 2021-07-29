using Inquirer.Data;
using Microsoft.AspNetCore.Identity;

namespace Inquirer.Controllers
{
    public sealed class SurveyController : EntityController<Survey, int>
    {
        public SurveyController(InquirerDbContext db) : base(db) { }

        protected override void NewEntity(Survey entity)
        {
            entity.RespondentId = HttpContext.User.GetId();
            entity.CreateTime = System.DateTime.Now;
        }
    }
}