using Doan_Web_CK.Models;
using Microsoft.EntityFrameworkCore;

namespace Doan_Web_CK.Repository
{
    public class EFMessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public EFMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = _context.Messages.SingleOrDefault(p => p.Id == id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task<Message> GetAsync(int id)
        {
            return await _context.Messages.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}
