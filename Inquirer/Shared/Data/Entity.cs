namespace Inquirer.Data
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
    public abstract class Entity : IEntity<int>
    {
        public int Id { get; set; }
        public bool IsNew => Id == 0;
        public abstract Entity ForDatabase();
    }
    public abstract class Entity<T> : Entity where T : Entity, new()
    {
        public override abstract T ForDatabase();
    }
}