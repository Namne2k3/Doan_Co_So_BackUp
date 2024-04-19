using Doan_Web_CK.Models;

namespace Doan_Web_CK.Repository
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task DeleteAsync(int id);
        Task UpdateAsync(Message message);
        Task<Message> GetAsync(int id);
    }
}
