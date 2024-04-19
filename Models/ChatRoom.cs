using System.ComponentModel.DataAnnotations;

namespace Doan_Web_CK.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }

        [Required]
        public string roomName { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? FriendId { get; set; }
        public ApplicationUser? Friend { get; set; }

        public List<Message>? Messages { get; set; }

        public string? ConnectionRoomCall { get; set; }
    }
}
