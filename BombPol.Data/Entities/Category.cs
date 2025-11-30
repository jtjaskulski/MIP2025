namespace BombPol.Data.Entities
{
    public class Category : SoftDeleteBusinessModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product>? Products { get; set; } = new List<Product>();

        public Category()
            : base(Guid.Empty)
        {
            
        }

        public Category(Guid id, string name, string description)
            :base(id)
        {
            Name = name;
            Description = description;
        }
    }
}