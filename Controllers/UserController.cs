using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vtc_Freelancer.Models;
using Vtc_Freelancer.Services;
using System;
namespace Vtc_Freelancer.Controllers
{
  public class UserController : Controller
  {
    private MyDbContext dbContext;
    private HashPassword hashPassword;
    private static Users user;
    private UserService userService;
    public UserController(MyDbContext dbContext, HashPassword hashPassword, UserService userService)
    {
      this.dbContext = dbContext;
      this.hashPassword = hashPassword;
      this.userService = userService;
      dbContext.Database.EnsureCreated();
    }
    [HttpPost("/Register")]
    public IActionResult Register(string username, string email, string password)
    {
      if (userService.Register(username, email, password))
      {
        ViewBag.Noti = true;
      }
      return Redirect("/Login");
    }
    [HttpGet("/Register")]
    public IActionResult Register()
    {
      return View();
    }
    [HttpPost("/Login")]
    public IActionResult Login(string email, string password)
    {
      user = new Users();
      user = userService.Login(email, password);
      if (user == null)
      {
        return Redirect("/Login");
      }
      HttpContext.Session.SetInt32("UserId", user.UserId);
      ViewBag.Notification = true;
      return Redirect("/");
    }
    [HttpGet("/Login")]
    public IActionResult Login()
    {
      return View();
    }
    public IActionResult Logout()
    {
      HttpContext.Session.Clear();
      return Redirect("/");
    }
    [HttpPost("/EditProfile")]
    public IActionResult EditProfile(int Id, string UserName, string Email)
    {
      bool Edit = userService.EditProfile(Id, UserName, Email);
      if (Edit == true)
      {
        return Redirect("/");
      }
      return View();
    }
    [HttpGet("/EditProfile")]
    public IActionResult EditProfile()
    {

      return View();
    }
  }
}