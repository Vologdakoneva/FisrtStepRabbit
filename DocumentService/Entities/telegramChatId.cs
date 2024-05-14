using System.ComponentModel.DataAnnotations;

namespace DocumentService.Entities
{
    public class telegramChat
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string ChatId { get; set; } = string.Empty;
    }
}
