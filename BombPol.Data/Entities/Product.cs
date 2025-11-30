namespace BombPol.Data.Entities
{
    public class Product : SoftDeleteBusinessModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public Product()
            : base(Guid.Empty)
        {
                
        }

        public Product(Guid id, string name, string description, decimal price, Guid categoryId)
            : base(id)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }
    }
}