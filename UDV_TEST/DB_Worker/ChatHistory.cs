using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UDV_TEST.DB_Worker
{
    public class ChatHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Id_Chat { get; set; }
        [ForeignKey("Id_Chat")]
        public Chat Chat { get; set; }
        [Required]
        public long TimeTicks { get; set; }
        [Required]
        public bool IsBot { get; set; }
        [Required]
        public string Message { get; set; }
    }
}