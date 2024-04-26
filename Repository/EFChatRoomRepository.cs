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
            var chatrooms = await _context.chatRooms.Where(p => p.Users[0].Id == id || p.Users[1].Id == id).ToListAsync();
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
                .Include(p => p.Users)
                .Include(p => p.Messages)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatRoom>> GetAllChatRoomByUserIdAsync(string userId)
        {
            var allCr = await _context.chatRooms.ToListAsync();
            var user = await _context.ApplicationUsers.SingleOrDefaultAsync(p => p.Id == userId);
            var chatrooms = await _context.chatRooms
                .Include(p => p.Users)
                .Include(p => p.Messages)
                .Where(p => p.Users.Contains(user))
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
                 .Include(p => p.Users)
                 .Include(p => p.Messages)
                 .SingleOrDefaultAsync(p => p.Id == id);

            chatroom.Messages = chatroom.Messages.OrderBy(p => p.Time).ToList();
            return chatroom;
        }

        public async Task<ChatRoom> GetByUsersIdAsync(string user1, string user2)
        {
            return await _context.chatRooms
                .Include(p => p.Users)
                .SingleOrDefaultAsync(
                    p => p.Users[0].Id == user1 && p.Users[1].Id == user2 ||
                    p.Users[0].Id == user2 && p.Users[1].Id == user1
                );
        }
    }
}
