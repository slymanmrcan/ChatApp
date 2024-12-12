using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers;

public class LoginController : BaseController
{
    // GET

    public IActionResult SimpleLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SimpleLogin(string username)
    {
        if (String.IsNullOrEmpty(username))
        {
            return View("Error");
        }

        return RedirectToAction("Index", "Chat");
    }
}
