using Doan_Web_CK.Models;

namespace Doan_Web_CK.Repository
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(string imagePath);
    }
}
