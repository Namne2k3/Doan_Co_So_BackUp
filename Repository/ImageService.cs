using System.Net;
using System.Net.Mail;

namespace Doan_Web_CK.Repository
{
    public class ImageService : IImageService
    {
        // Hàm lưu ảnh từ đường dẫn
        public async Task<string> SaveImageAsync(string imageUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Tải ảnh từ URL
                    HttpResponseMessage response = await httpClient.GetAsync(imageUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung của phản hồi vào một mảng byte
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                        // Tạo tên tệp mới cho ảnh (có thể sử dụng Guid để tạo tên duy nhất)
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(new Uri(imageUrl).LocalPath);

                        // Đường dẫn thư mục lưu trữ ảnh trên máy chủ (thay thế bằng đường dẫn thư mục của bạn)
                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        // Lưu ảnh vào thư mục lưu trữ
                        await File.WriteAllBytesAsync(uploadPath, imageBytes);

                        // Trả về đường dẫn của ảnh đã lưu
                        return "/images/" + fileName;
                    }
                    else
                    {
                        Console.WriteLine("Lỗi khi tải ảnh: " + response.StatusCode);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine("Lỗi khi lưu ảnh: " + ex.Message);
                return null;
            }
        }
    }
}