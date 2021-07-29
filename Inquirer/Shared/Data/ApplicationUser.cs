using Microsoft.AspNetCore.Identity;

namespace Inquirer.Data
{
    public sealed class ApplicationUser : IdentityUser, IEntity<string> { }
}