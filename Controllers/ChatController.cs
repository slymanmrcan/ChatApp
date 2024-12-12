using ChatApp.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers;

public class ChatController(ChatHubBusiness chatHubBusiness) : BaseController
{
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ChatScreen()
    {
        return View();
    }
}