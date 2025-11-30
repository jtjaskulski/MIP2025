namespace BombPol.Api.Messages.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
