using System.ComponentModel.DataAnnotations;

namespace UDV_TEST.DB_Worker
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<ChatHistory> Messages { get; set; }
    }
}
