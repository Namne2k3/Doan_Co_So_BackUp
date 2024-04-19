using Doan_Web_CK.Models;
using Doan_Web_CK.Repository;
using Microsoft.AspNetCore.SignalR;
namespace Doan_Web_CK.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IAccountRepository _accountRepository;
        public ChatHub(IChatRoomRepository chatRoomRepository, IMessageRepository messageRepository, IAccountRepository accountRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _messageRepository = messageRepository;
            _accountRepository = accountRepository;
        }
        public async Task SendToastMessageNof(string userId, string receiverId, string message, string chatRoomId)
        {
            var account = await _accountRepository.GetByIdAsync(userId);
            var receiver = await _accountRepository.GetByIdAsync(receiverId);

            await Clients.User(receiver.Id).SendAsync("ReceiveToastMessageNof", account.UserName, message, account.ImageUrl, chatRoomId);
        }
        public async Task SendToastMessage(string userId, string receiverId, string connectionRoomCall)
        {
            var account = await _accountRepository.GetByIdAsync(userId);
            var receiver = await _accountRepository.GetByIdAsync(receiverId);

            await Clients.User(receiver.Id).SendAsync("ReceiveToastMessage", account.UserName, connectionRoomCall, account.ImageUrl);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendCallMessageToUser(string user, string receiverConnectionId, string message, string connectionRoomCall)
        {
            if (message != "")
            {
                var sender = await _accountRepository.GetByIdAsync(user);
                var receiver = await _accountRepository.GetByIdAsync(receiverConnectionId);
                var time = DateTime.Now;
                var chatroom = await _chatRoomRepository.GetByUsersIdAsync(sender.Id, receiver.Id);
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

                    await Clients.User(user).SendAsync("ReceiveMessage", sender.UserName, message, sender.ImageUrl, "right", time.ToString(), newMessage.Type, connectionRoomCall);
                    await Clients.User(receiverConnectionId).SendAsync("ReceiveMessage", receiver.UserName, message, sender.ImageUrl, "left", time.ToString(), newMessage.Type, connectionRoomCall);
                }
            }
        }
        public async Task SendToUser(string user, string receiverConnectionId, string message)
        {
            if (message != "")
            {
                var sender = await _accountRepository.GetByIdAsync(user);
                var receiver = await _accountRepository.GetByIdAsync(receiverConnectionId);
                var time = DateTime.Now;
                var chatroom = await _chatRoomRepository.GetByUsersIdAsync(sender.Id, receiver.Id);
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
                    };
                    await _chatRoomRepository.AddMessagesAsync(chatroom, newMessage);
                    time = newMessage.Time;
                }
                await Clients.User(user).SendAsync("ReceiveMessage", user, message, sender.ImageUrl, "right", time.ToString());
                await Clients.User(receiverConnectionId).SendAsync("ReceiveMessage", receiverConnectionId, message, sender.ImageUrl, "left", time.ToString());
            }
        }

        public string GetConnectionId() => Context.UserIdentifier;
    }
}
