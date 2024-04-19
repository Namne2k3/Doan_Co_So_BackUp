using Doan_Web_CK.Models;
using Doan_Web_CK.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doan_Web_CK
{
    [Authorize]
    public class CallController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotifiticationRepository _notifiticationRepository;
        private readonly IFriendShipRepository _friendShipRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        public CallController(
            UserManager<ApplicationUser> userManager
            , IAccountRepository accountRepository
            , IFriendShipRepository friendShipRepository
            , INotifiticationRepository notifiticationRepository
            , IChatRoomRepository chatRoomRepository
            )
        {
            _userManager = userManager;
            _chatRoomRepository = chatRoomRepository;
            _notifiticationRepository = notifiticationRepository;
            _friendShipRepository = friendShipRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet("/call/{roomId}")]
        public async Task<IActionResult> Room(string roomId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var account = await _accountRepository.GetByIdAsync(currentUser.Id);

            var chatRooms = await _chatRoomRepository.GetAllAsync();

            var currentChatRoom = chatRooms.Where(p =>
                p.ConnectionRoomCall == roomId
                &&
                p.UserId == currentUser.Id || p.FriendId == currentUser.Id
            ).ToList();

            var findedChatroom = currentChatRoom.SingleOrDefault(p => p.ConnectionRoomCall == roomId);

            if (findedChatroom == null)
            {
                return NotFound();
            }
            ViewBag.currentChatroom = findedChatroom;
            ViewBag.CurrentUser = account;
            ViewBag.roomId = roomId;
            ViewBag.GetUserName = new Func<string, string>(GetUserName);
            ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
            ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
            return View();
        }
        public async Task<bool> IsRequestedAsync(string userId, string friendId)
        {
            var friendships = await _friendShipRepository.GetAllAsync();
            var finded = friendships.SingleOrDefault(p => p.UserId == userId && p.FriendId == friendId || p.UserId == friendId && p.FriendId == userId && p.IsConfirmed == false);
            if (finded != null)
            {
                return true;
            }
            return false;
        }
        public bool IsRequested(string userId, string friendId)
        {
            var task = IsRequestedAsync(userId, friendId);
            task.Wait();
            return task.Result;
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
    }

}
