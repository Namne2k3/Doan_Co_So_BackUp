using Doan_Web_CK.Models;
using Microsoft.EntityFrameworkCore;

namespace Doan_Web_CK.Repository
{
    public class EFChatRoomRepository : IChatRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public EFChatRoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatRoom chatRoom)
        {
            _context.chatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();
        }

        public async Task AddMessagesAsync(ChatRoom chatroom, Message message)
        {
            if (chatroom.Messages == null)
            {
                chatroom.Messages = new List<Message>();
            }
            chatroom.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatRoomsByUserId(string id)
        {
            var chatrooms = await _context.chatRooms.Where(p => p.UserId == id || p.FriendId == id).ToListAsync();
            if (chatrooms != null)
            {
                foreach (var item in chatrooms)
                {
                    _context.chatRooms.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<ChatRoom>> GetAllAsync()
        {
            return await _context.chatRooms
                .Include(p => p.User)
                .Include(p => p.Friend)
                .Include(p => p.Messages)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatRoom>> GetAllChatRoomByUserIdAsync(string userId)
        {
            var chatrooms = await _context.chatRooms
                .Include(p => p.User)
                .Include(p => p.Friend)
                .Include(p => p.Messages)
                .Where(p => p.UserId == userId || p.FriendId == userId)
                .ToListAsync();

            foreach (var cr in chatrooms)
            {
                cr.Messages = cr.Messages.OrderBy(p => p.Time).ToList();
            }
            return chatrooms;
        }

        public async Task<ChatRoom> GetByIdAsync(int id)
        {
            var chatroom = await _context.chatRooms
                 .Include(p => p.User)
                 .Include(p => p.Friend)
                 .Include(p => p.Messages)
                 .SingleOrDefaultAsync(p => p.Id == id);

            chatroom.Messages = chatroom.Messages.OrderBy(p => p.Time).ToList();
            return chatroom;
        }

        public async Task<ChatRoom> GetByUsersIdAsync(string user1, string user2)
        {
            return await _context.chatRooms
                .Include(p => p.User)
                .Include(p => p.Friend)
                .SingleOrDefaultAsync(
                    p => p.UserId == user1 && p.FriendId == user2 ||
                    p.UserId == user2 && p.FriendId == user1
                );
        }
    }
}
