using Doan_Web_CK.Models;

namespace Doan_Web_CK.Repository
{
    public interface IChatRoomRepository
    {
        Task<IEnumerable<ChatRoom>> GetAllAsync();

        Task<ChatRoom> GetByIdAsync(int id);

        Task AddAsync(ChatRoom chatRoom);

        Task<ChatRoom> GetByUsersIdAsync(string user1, string user2);

        Task<IEnumerable<ChatRoom>> GetAllChatRoomByUserIdAsync(string userId);

        Task AddMessagesAsync(ChatRoom chatroom, Message message);
        Task DeleteChatRoomsByUserId(string id);
    }
}
