using Doan_Web_CK.Models;
using Doan_Web_CK.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Doan_Web_CK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IChatRoomRepository _chatRoomRepository;
        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, IChatRoomRepository chatRoomRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _chatRoomRepository = chatRoomRepository;
        }

        public async Task<IActionResult> Index()
        {
            var account = await _accountRepository.GetByIdAsync(_userManager.GetUserId(User));
            ViewBag.currentUser = account;
            ViewBag.ChatRooms = await _chatRoomRepository.GetAllChatRoomByUserIdAsync(account?.Id);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
