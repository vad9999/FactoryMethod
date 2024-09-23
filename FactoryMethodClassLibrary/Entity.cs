namespace FactoryMethodClassLibrary
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        internal Entity(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}