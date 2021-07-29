using Inquirer.Data;

namespace Inquirer.Controllers
{
    public sealed class ApplicationUserController : EntityController<ApplicationUser, string>
    {
        public ApplicationUserController(InquirerDbContext db) : base(db) { }
    }
}