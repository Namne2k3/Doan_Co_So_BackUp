using System.Security.Principal;
using Doan_Web_CK.Models;
using Doan_Web_CK.Repository;
using Google.Protobuf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
namespace Doan_Web_CK.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatHub(IWebHostEnvironment environment, IChatRoomRepository chatRoomRepository, IAccountRepository accountRepository, UserManager<ApplicationUser> userManager)
        {
            _environment = environment;
            _userManager = userManager;
            _chatRoomRepository = chatRoomRepository;
            _accountRepository = accountRepository;
        }
        public async Task SendToastMessageNof(string userId, string message, string chatRoomId)
        {
            var account = await _accountRepository.GetByIdAsync(userId);
            var currentUser = await _accountRepository.GetByIdAsync(userId);
            var chatRoomGroup = await _chatRoomRepository.GetByIdAsync(int.Parse(chatRoomId));
            foreach (var user in chatRoomGroup.Users)
            {
                if (user.Id != currentUser.Id)
                {
                    await Clients.User(user.Id).SendAsync("ReceiveToastMessageNof", account.UserName, message, account.ImageUrl, chatRoomId);
                }
            }
        }
        public async Task SendToastMessage(string userId, string receiverId, string connectionRoomCall, string chatRoomId)
        {
            var currentUser = await _accountRepository.GetByIdAsync(userId);
            var account = await _accountRepository.GetByIdAsync(userId);
            var receiver = await _accountRepository.GetByIdAsync(receiverId);
            var chatroom = await _chatRoomRepository.GetByIdAsync(int.Parse(chatRoomId));

            foreach (var mb in chatroom.Users)
            {
                if (mb.Id != currentUser.Id)
                {
                    await Clients.User(mb.Id).SendAsync("ReceiveToastMessage", account.UserName, connectionRoomCall, account.ImageUrl);
                }
            }
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendCallMessageToUser(string user, string receiverConnectionId, string message, string connectionRoomCall, string chatRoomId)
        {
            var currentUser = await _accountRepository.GetByIdAsync(user);
            if (message != "")
            {
                var sender = await _accountRepository.GetByIdAsync(user);
                var receiver = await _accountRepository.GetByIdAsync(receiverConnectionId);
                var time = DateTime.Now;
                var chatroom = await _chatRoomRepository.GetByIdAsync(int.Parse(chatRoomId));
                if (chatroom != null)
                {
                    var newMessage = new Message
                    {
                        UserName = sender.UserName,
                        UserImageUrl = sender.ImageUrl,
                        Text = message,
                        Time = DateTime.Now,
                        userId = sender.Id,
                        ApplicationUser = sender,
                        ChatRoomId = chatroom.Id,
                        Type = "call",
                        connectionRoomCall = connectionRoomCall,
                    };
                    await _chatRoomRepository.AddMessagesAsync(chatroom, newMessage);
                    time = newMessage.Time;

                    foreach (var mb in chatroom.Users)
                    {
                        if (mb.Id == currentUser.Id)
                        {
                            await Clients.User(mb.Id).SendAsync("ReceiveMessage", sender.UserName, message, sender.ImageUrl, "right", time.ToString(), chatroom.Id, newMessage.Type, connectionRoomCall);

                        }
                        else
                        {
                            await Clients.User(mb.Id).SendAsync("ReceiveMessage", receiver.UserName, message, sender.ImageUrl, "left", time.ToString(), chatroom.Id, newMessage.Type, connectionRoomCall);
                        }
                    }
                }
            }
        }
        public async Task UnSendMessageToUser(string userId, int messageId, int chatRoomId)
        {
            var currentUser = await _accountRepository.GetByIdAsync(userId);
            var chatRoom = await _chatRoomRepository.GetByIdAsync(chatRoomId);
            if (messageId != null & chatRoomId != null)
            {
                foreach (var user in chatRoom.Users)
                {
                    Clients.User(user.Id).SendAsync("ReceiveUnsendMessage", userId, messageId, chatRoomId);
                }
            }
        }
        public async Task SendToUser(string user, string message, string chatRoomId, List<string> arrayImageMessages)
        {
            var currentUser = await _accountRepository.GetByIdAsync(user);
            var chatRoomGroup = await _chatRoomRepository.GetByIdAsync(int.Parse(chatRoomId));

            var sender = await _accountRepository.GetByIdAsync(user);
            var time = DateTime.Now;
            var msId = "";
            if (chatRoomGroup != null)
            {
                var newMessage = new Message
                {
                    UserName = sender.UserName,
                    UserImageUrl = sender.ImageUrl,
                    Text = message,
                    Time = DateTime.Now,
                    userId = sender.Id,
                    ApplicationUser = sender,
                    ChatRoomId = chatRoomGroup.Id,
                };
                if (arrayImageMessages.Count > 0)
                {
                    if (newMessage.MessageImages == null)
                    {
                        newMessage.MessageImages = new List<string>();
                    }
                    newMessage.Type = "image";
                    foreach (var img in arrayImageMessages)
                    {
                        newMessage.MessageImages.Add(img);
                    }
                }
                await _chatRoomRepository.AddMessagesAsync(chatRoomGroup, newMessage);
                time = newMessage.Time;
                msId = newMessage.Id.ToString();

                foreach (var userItem in chatRoomGroup.Users)
                {
                    if (userItem.Id == currentUser.Id)
                    {
                        await Clients.User(userItem.Id).SendAsync("ReceiveMessage", userItem.Id, message, sender.ImageUrl, "right", time.ToString(), chatRoomGroup.Id, newMessage.Type, "", msId, newMessage.MessageImages);
                    }
                    else
                    {
                        await Clients.User(userItem.Id).SendAsync("ReceiveMessage", userItem.Id, message, sender.ImageUrl, "left", time.ToString(), chatRoomGroup.Id, newMessage.Type, "", msId, newMessage.MessageImages);
                    }
                }
            }


        }
        public async Task UploadImage(string imageData)
        {
            try
            {
                // Parse the data URI and extract the base64-encoded data
                var base64Data = imageData.Split(",")[1];

                // Convert base64-encoded data to byte array
                var bytes = Convert.FromBase64String(base64Data);

                // Generate unique file name
                var fileName = $"{Guid.NewGuid().ToString()}.png";

                // Get the absolute path to the wwwroot/images directory
                var imagePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                // Write the byte array to the file
                await File.WriteAllBytesAsync(imagePath, bytes);

                // Notify clients about the new image
                await Clients.Caller.SendAsync("ImageUploaded", $"/images/{fileName}");
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error uploading image: {ex.Message}");
            }
        }
        public string GetConnectionId() => Context.UserIdentifier;
    }
}
