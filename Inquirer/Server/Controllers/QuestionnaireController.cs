using Inquirer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Inquirer.Controllers
{
    public sealed class QuestionnaireController : EntityController<Questionnaire, int>
    {
        public QuestionnaireController(InquirerDbContext db) : base(db) { }

        protected override IQueryable<Questionnaire> GetQuery([CallerMemberName] string methodName = null)
        {
            switch (methodName)
            {
                case nameof(Get):
                    return DbContext.Questionnaires
                        .Include(t => t.Groups)
                        .Include(t => t.Questions)
                        .ThenInclude(t => t.Answers);
                case nameof(GetAll):
                    return DbContext.Questionnaires
                        .Include(t => t.Author);
            }
            return base.GetQuery(methodName);
        }

        protected override void NewEntity(Questionnaire entity)
        {
            entity.CreateTime = DateTime.Now;
        }
    }
}