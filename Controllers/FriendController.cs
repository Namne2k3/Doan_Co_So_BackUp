using Doan_Web_CK.Models;
using Doan_Web_CK.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Doan_Web_CK.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly IFriendShipRepository _friendShipRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotifiticationRepository _notificationRepository;

        public FriendController(IFriendShipRepository friendShipRepository, IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, INotifiticationRepository notificationRepository)
        {
            _friendShipRepository = friendShipRepository;
            _accountRepository = accountRepository;
            _userManager = userManager;
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SearchMembers(string query)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var friends = await _friendShipRepository.GetAllAsync();
            var filtered = friends.Where(p => p.UserId == currentUser.Id || p.FriendId == currentUser.Id).ToList();
            filtered = filtered.Where(p => p.IsConfirmed == true).ToList();
            if (query != null)
            {
                filtered = filtered.Where(p => p.User.UserName.ToLower().Contains(query.ToLower()) || p.Friend.UserName.ToLower().Contains(query.ToLower())).ToList();
            }

            if (filtered.Count == 0)
            {
                return Json(new
                {
                    message = "not found",
                });
            }
            return Json(new
            {
                message = "found",
                html = GenerateFriendCards(filtered, currentUser.Id)
            });

        }
        public string GenerateFriendCards(List<Friendship> friends, string currentUser)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var friend in friends)
            {
                sb.Append("<div class=\"create_friend_card\">");
                sb.Append("<div class=\"create_friend_img_container\">");
                if (friend.UserId == currentUser)
                {
                    sb.Append($"<img style=\"width: 100%; height: 100%;\" src=\"{friend.Friend?.ImageUrl}\" alt=\"friend_card_image\" />");
                }
                else
                {
                    sb.Append($"<img style=\"width: 100%; height: 100%;\" src=\"{friend.User?.ImageUrl}\" alt=\"friend_card_image\" />");
                }
                sb.Append("</div>");

                sb.Append("<div class=\"create_friend_action\">");
                if (friend.UserId == currentUser)
                {
                    sb.Append($"<a class=\"create_friend_username\" asp-controller=\"Profile\" asp-action=\"Index\" asp-route-id=\"{friend.Friend?.Id}\">{friend.Friend?.UserName}</a>");
                    sb.Append($"<a onclick=\"handleAddUserToGroup('{friend.Friend.Id}', '{friend.Friend.ImageUrl}', '{friend.Friend.UserName}')\" class=\"btn btn-outline-dark\">Add User</a>");
                }
                else
                {
                    sb.Append($"<a class=\"create_friend_username\" asp-controller=\"Profile\" asp-action=\"Index\" asp-route-id=\"{friend.User?.Id}\">{friend.User?.UserName}</a>");
                    sb.Append($"<a onclick=\"handleAddUserToGroup('{friend.User.Id}', '{friend.User.ImageUrl}', '{friend.User.UserName}')\" class=\"btn btn-outline-dark\">Add User</a>");
                }
                sb.Append("</div>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }
        public async Task<IActionResult> UnFriend(string userId, string friendId)
        {
            var currnentUser = await _userManager.GetUserAsync(User);
            var friendShips = await _friendShipRepository.GetAllAsync();
            var finded = friendShips.SingleOrDefault(p => p.UserId == userId && p.FriendId == friendId || p.UserId == friendId && p.FriendId == userId);
            StringBuilder sbProfile = new StringBuilder();
            StringBuilder sbBlogIndex = new StringBuilder();

            StringBuilder sbFriendIndex = new StringBuilder();
            if (finded != null && finded.IsConfirmed == true)
            {
                await _friendShipRepository.DeleteAsync(finded.Id);
                //friendShips = await _friendShipRepository.GetAllAsync();
                //foreach (var f in friendShips)
                //{
                if (currnentUser.Id == userId)
                {
                    sbFriendIndex.Append("<a class=\"btn btn-outline-light\" href=\"/Profile/Index/" + friendId + "\" >View Profile</a>");
                }
                else
                {
                    sbFriendIndex.Append("<a class=\"btn btn-dark\" href=\"/Profile/Index/" + userId + "\" >View Profile</a>");
                }
                sbFriendIndex.Append("<a class=\"btn btn-outline-light disabled\">Add friend</a>");
                //}

                //<a asp - action = "Index" asp - controller = "Profile" asp - route - id = "@item.AccountId" class="btn btn-dark">View Profile</a>
                //<a onclick = "handleAddFriend('@currentUser.Id', '@item.Id')" class="btn btn-dark">Add Friend</a>
                sbBlogIndex.Append("<a class=\"btn btn-dark\" href=\"/Profile/Index/" + friendId + "\" >View Profile</a>");
                sbBlogIndex.Append("<a class=\"btn btn-dark disabled\">Add friend</a>");

                sbProfile.Append("<div>");
                sbProfile.Append("<a class=\"btn btn-outline-light disabled\">Add friend</a>");
                sbProfile.Append("</div>");




                return Json(new
                {
                    message = "success",
                    sbProfile = sbProfile.ToString(),
                    sbBlogIndex = sbBlogIndex.ToString(),
                    sbFriendIndex = sbFriendIndex.ToString()
                });
            }
            else
            {
                return Json(new
                {
                    message = "failed"
                });
            }
        }

        public async Task<IActionResult> Index(string search)
        {
            if (search == null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var account = await _accountRepository.GetByIdAsync(currentUser.Id);
                ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
                ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
                ViewBag.GetUserName = new Func<string, string>(GetUserName);
                ViewBag.currentUser = account;
                var friends = await _friendShipRepository.GetAllAsync();
                var filtered = friends.Where(p => p.UserId == account.Id || p.FriendId == account.Id).ToList();
                return View(filtered);
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var account = await _accountRepository.GetByIdAsync(currentUser.Id);
                ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
                ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
                ViewBag.GetUserName = new Func<string, string>(GetUserName);
                ViewBag.currentUser = account;
                ViewBag.Search = search;
                var friends = await _friendShipRepository.GetAllAsync();
                var filtered = friends.Where(
                    p => p.UserId == account.Id && p.Friend.UserName.ToLower().Contains(search.ToLower())
                    || p.FriendId == account.Id && p.User.UserName.ToLower().Contains(search.ToLower())
                ).ToList();
                return View(filtered);
            }
        }
        public async Task<bool> IsFriendAsync(string userId, string friendId)
        {
            var friendship = await _friendShipRepository.GetAllAsync();
            var finded = friendship.SingleOrDefault(p => p.UserId == userId && p.FriendId == friendId || p.UserId == friendId && p.FriendId == userId);
            if (finded != null && finded.IsConfirmed == true)
            {
                return true;
            }
            return false;
        }
        public bool IsFriend(string userId, string friendId)
        {
            var task = IsFriendAsync(userId, friendId);
            task.Wait();
            return task.Result;
        }
        public async Task<IActionResult> Friend(string search)
        {
            if (search == null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var account = await _accountRepository.GetByIdAsync(currentUser.Id);
                ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
                ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
                ViewBag.GetUserName = new Func<string, string>(GetUserName);
                ViewBag.IsFriend = new Func<string, string, bool>(IsFriend);
                ViewBag.currentUser = account;
                var friends = await _accountRepository.GetAllAsync();
                var filterd = friends.Where(p => p.Id != currentUser.Id).ToList();
                return View(filterd);
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var account = await _accountRepository.GetByIdAsync(currentUser.Id);
                ViewBag.GetAllNofOfUser = new Func<string, IEnumerable<Nofitication>>(GetAllNofOfUser);
                ViewBag.IsRequested = new Func<string, string, bool>(IsRequested);
                ViewBag.GetUserName = new Func<string, string>(GetUserName);
                ViewBag.currentUser = account;
                ViewBag.Search = search;
                ViewBag.IsFriend = new Func<string, string, bool>(IsFriend);
                var friends = await _accountRepository.GetAllAsync();
                var filterd = friends.Where(p => p.UserName.ToLower().Contains(search.ToLower()) && p.Id != currentUser.Id);
                return View(filterd);
            }
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
        public async Task<IEnumerable<Nofitication>> GetAllNofOfUserAsync(string userId)
        {
            var user = await _accountRepository.GetByIdAsync(userId);

            var nofitications = await _notificationRepository.GetAllNotifitions();
            var filtered = nofitications.Where(p => p.RecieveAccountId == userId).ToList();
            return filtered;
        }
        public IEnumerable<Nofitication> GetAllNofOfUser(string userId)
        {
            var task = GetAllNofOfUserAsync(userId);
            task.Wait();
            return task.Result;
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
        public bool IsRequested(string userId, string friendId)
        {
            var task = IsRequestedAsync(userId, friendId);
            task.Wait();
            return task.Result;
        }
    }
}
