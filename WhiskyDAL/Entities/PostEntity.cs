namespace WhiskyDAL.Entities
{
    public class PostEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WhiskyId { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public DateTime DateTime { get; set; }

        public UserEntity User { get; set; }
        public WhiskyEntity Whisky { get; set; }
    }
} 