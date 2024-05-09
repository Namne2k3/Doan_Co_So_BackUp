using Doan_Web_CK.Models;
using Doan_Web_CK.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doan_Web_CK.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IFriendShipRepository _friendShipRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly INotifiticationRepository _notifiticationRepository;

        public async Task AddMember(ChatRoom chatRoom, IEnumerable<string> members)
        {

            foreach (string id in members)
            {
                chatRoom.Users.Add(await _accountRepository.GetByIdAsync(id));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(string roomName, List<string> members)
        {
            var chatRoom = new ChatRoom
            {
                roomName = roomName,
                Users = new List<ApplicationUser>(),
                ConnectionRoomCall = Guid.NewGuid().ToString(),
                ChatRoomImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDXWIPPDsd2vImhX-d9OHdABO2iwj3yS9y_6YEVKpi3A&s"
            };

            await AddMember(chatRoom, members);
            await _chatRoomRepository.AddAsync(chatRoom);

            if (chatRoom.Users.Count < 3)
            {
                return Json(new
                {
                    message = "failed"
                });
            }

            return RedirectToAction("Group");
        }
        public async Task<IActionResult> Create()
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            var friends = await _friendShipRepository.GetAllAsync();
            var filtered = friends.Where(p => p.UserId == account.Id || p.FriendId == account.Id).ToList();
            filtered = filtered.Where(p => p.IsConfirmed == true).ToList();
            ViewBag.currentUser = account;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.Friends = filtered;
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            var ownChatRoom = await _chatRoomRepository.GetAllChatRoomByUserIdAsync(account?.Id);
            return View();
        }
        public ChatController(UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IMessageRepository messageRepository, IChatRoomRepository chatRoomRepository, IFriendShipRepository friendShipRepository, INotifiticationRepository notifiticationRepository)
        {
            _userManager = userManager;
            _accountRepository = accountRepository;
            _messageRepository = messageRepository;
            _chatRoomRepository = chatRoomRepository;
            _friendShipRepository = friendShipRepository;
            _notifiticationRepository = notifiticationRepository;
        }
        public async Task<bool> IsRequestedAsync(string userId, string friendId)
        {
            var friendships = await _friendShipRepository.GetAllAsync();
            var finded = friendships.SingleOrDefault(p => p.UserId == userId && p.FriendId == friendId && p.IsConfirmed == false);
            if (finded != null)
            {
                return true;
            }
            return false;
        }
        [HttpGet]
        public async Task<IEnumerable<Nofitication>> GetAllNofOfUserAsync(string userId)
        {
            var user = await _accountRepository.GetByIdAsync(userId);

            var nofitications = await _notifiticationRepository.GetAllNotifitions();
            var filtered = nofitications.Where(p => p.RecieveAccountId == userId).ToList();
            return filtered;
        }
        public IEnumerable<Nofitication> GetAllNofOfUser(string userId)
        {
            var task = GetAllNofOfUserAsync(userId);
            task.Wait();
            return task.Result;
        }
        public bool IsRequested(string userId, string friendId)
        {
            var task = IsRequestedAsync(userId, friendId);
            task.Wait();
            return task.Result;
        }
        public async Task<string> GetUserNameByIdAsync(string id)
        {
            var user = await _accountRepository.GetByIdAsync(id);
            return "@" + user.UserName;
        }
        public string GetUserName(string id)
        {
            var task = GetUserNameByIdAsync(id);
            task.Wait();
            return task.Result;
        }


        [HttpGet]
        public async Task<IActionResult> GetChatRoomId(string userId, string friendId)
        {
            var chatRooms = await _chatRoomRepository.GetAllAsync();
            var user = await _accountRepository.GetByIdAsync(userId);
            var friend = await _accountRepository.GetByIdAsync(friendId);
            var finded = chatRooms.FirstOrDefault(p => p.Users.Contains(user) && p.Users.Contains(friend) && p.ChatRoomImage == null);
            if (finded != null)
            {
                return Json(new
                {
                    message = "found",
                    chatRoomId = finded.Id
                });
            }
            else
            {
                var chatRoom = new ChatRoom
                {
                    roomName = friend.UserName,
                    Users = new List<ApplicationUser>(),
                    Messages = new List<Message>(),
                    ConnectionRoomCall = Guid.NewGuid().ToString(),
                };
                chatRoom.Users.Add(user);
                chatRoom.Users.Add(friend);

                await _chatRoomRepository.AddAsync(chatRoom);

                return Json(new
                {
                    message = "found",
                    chatRoomId = chatRoom.Id
                });
            }
        }
        public async Task<IActionResult> DetailsGroup(int id)
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            var chatrooms = await _chatRoomRepository.GetAllAsync();
            ViewBag.currentUser = account;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            var ownChatRoom = await _chatRoomRepository.GetAllChatRoomByUserIdAsync(account?.Id);
            ownChatRoom = ownChatRoom.Where(p => p.ChatRoomImage != null).ToList();
            var currentChatRoom = await _chatRoomRepository.GetByIdAsync(id);
            ViewBag.currentChatRoom = currentChatRoom;
            return View(ownChatRoom);
        }
        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            var chatrooms = await _chatRoomRepository.GetAllAsync();
            ViewBag.currentUser = account;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            var ownChatRoom = await _chatRoomRepository.GetAllChatRoomByUserIdAsync(account?.Id);
            ownChatRoom = ownChatRoom.Where(p => p.ChatRoomImage == null).ToList();
            var currentChatRoom = await _chatRoomRepository.GetByIdAsync(id);
            ViewBag.currentChatRoom = currentChatRoom;
            return View(ownChatRoom);
        }
        public async Task<IActionResult> Group()
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            var chatrooms = await _chatRoomRepository.GetAllAsync();
            ViewBag.currentUser = account;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            var ownChatRoom = await _chatRoomRepository.GetAllChatRoomByUserIdAsync(account?.Id);
            ownChatRoom = ownChatRoom.Where(p => p.ChatRoomImage != null).ToList();
            var currentChatRoom = ownChatRoom.FirstOrDefault();
            ViewBag.currentChatRoom = currentChatRoom;
            if (currentChatRoom == null)
            {
                return View();
            }

            return View(ownChatRoom);
        }
        public async Task<IActionResult> Index()
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            var chatrooms = await _chatRoomRepository.GetAllAsync();
            ViewBag.currentUser = account;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            var ownChatRoom = await _chatRoomRepository.GetAllChatRoomByUserIdAsync(account?.Id);
            ownChatRoom = ownChatRoom.Where(p => p.ChatRoomImage == null).ToList();
            var currentChatRoom = ownChatRoom.FirstOrDefault();
            ViewBag.currentChatRoom = currentChatRoom;
            if (ownChatRoom == null)
            {
                return View();
            }
            return View(ownChatRoom);
        }

        public async Task<IActionResult> GetMessages(int roomId)
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            var room = await _chatRoomRepository.GetByIdAsync(roomId);
            ViewBag.currentUser = account;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            return View(room.Messages);
        }
    }
}
