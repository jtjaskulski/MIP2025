namespace BombPol.Data.Entities
{
    public abstract class SoftDeleteBusinessModel
    {
        public Guid Id { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        protected SoftDeleteBusinessModel(Guid id)
        {
                Id = id;
        }
    }
}