using System.ComponentModel.DataAnnotations;

namespace Doan_Web_CK.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string UserImageUrl { get; set; }

        [Required]
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public string userId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom? ChatRoom { get; set; }

        public string? Type { get; set; }
        public string? connectionRoomCall { get; set; }
    }
}
