using Inquirer.Data;

namespace Inquirer.Models
{
    public sealed class SwapRequestData<T> where T : Entity, IOrderedEntity
    {
        public T Entity1;
        public T Entity2;
    }
}