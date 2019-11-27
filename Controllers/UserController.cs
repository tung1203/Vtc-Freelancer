using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vtc_Freelancer.Models;
using Vtc_Freelancer.Services;
using System.Linq;
using System.Collections.Generic;

namespace Vtc_Freelancer.Controllers
{
  public class UserController : Controller
  {
    private MyDbContext dbContext;
    private UserService userService;
    private AdminService adminService;
    public UserController(MyDbContext dbContext, UserService userService, AdminService adminService)
    {

      this.dbContext = dbContext;
      this.userService = userService;
      this.adminService = adminService;
      dbContext.Database.EnsureCreated();
    }
    public IActionResult Index()
    {
      var userId = HttpContext.Session.GetInt32("UserId");
      return View();
    }
    [HttpPost("/Register")]
    public IActionResult Register(string username, string email, string password)
    {
      if (userService.Register(username, email, password))
      {
        ViewBag.Noti = "Register Successfully :)";
      }
      return Redirect("/Login");
    }
    [HttpGet("/Register")]
    public IActionResult Register()
    {
      List<Category> listcategory = new List<Category>();
      listcategory = adminService.GetListCategoryBy();
      if (listcategory != null)
      {
        ViewBag.listcategory = listcategory;

        return View();
      }
      return View();
    }

    [HttpPost("/Login")]

    public IActionResult Login(string email, string password)
    {
      Users user = new Users();
      user = userService.Login(email, password);
      if (user != null)
      {
        HttpContext.Session.SetString("UserName", user.UserName);
      }
      if (user == null)
      {
        ViewBag.Error = "Wrong Username/Email";
        ViewBag.Error1 = "Wrong Password";
        return View("Login");
      }
      else
      {
        if (user.UserLevel == 1)
        {
          HttpContext.Session.SetString("UserName", user.UserName);
          HttpContext.Session.SetInt32("UserId", user.UserId);
          HttpContext.Session.SetInt32("IsSeller", user.IsSeller);
          ViewBag.Notification = true;
        }
        if (user.Status == 0)
        {
          ViewBag.Error = "Account locked";
          if (user.UserLevel == 1)
          {
            return Redirect("/Admin/Dashboard");
          }
          return Redirect("/");
        }
      }
      ViewBag.Accountlocked = "Account locked";
      return View("Login");
    }
    [HttpGet("/Login")]
    public IActionResult Login()
    {
      List<Category> listcategory = new List<Category>();
      listcategory = adminService.GetListCategoryBy();
      var userId = HttpContext.Session.GetInt32("UserId");
      Users user = userService.GetUsersByID(userId);
      if (userId != null)
      {
        if (user.Status == 1)
        {
          // ViewBag.Accountlocked = "Account locked";
        }
      }
      else
      {
        // ViewBag.Error = "Wrong Username/email";
        // ViewBag.Error1 = "Wrong Passwowrd";
      }
      return View();
    }
    [HttpPost("/BecomeSeller")]
    public IActionResult BecomeSeller(Seller seller1, Languages languages, Category category, Skills skills)
    {

      Console.WriteLine(category.CategoryName);
      int? userId = HttpContext.Session.GetInt32("UserId");
      Users users = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
      var category1 = dbContext.Category.FirstOrDefault(cat => cat.CategoryName == category.CategoryName);

      Console.WriteLine(languages.Level);
      var seller = userService.BecomeSeller(users, languages, seller1, category1, skills);
      List<Category> listcategory = new List<Category>();
      listcategory = adminService.GetListCategoryBy();
      if (seller != null)
      {
        seller = dbContext.Seller.FirstOrDefault(seller => seller.SellerId == seller1.SellerId);
        // Set Session lan 2
        HttpContext.Session.SetInt32("IsSeller", users.IsSeller);

        return Redirect("/Home/Index");
      }
      return View();
    }

    [HttpGet("/BecomeSeller")]
    public IActionResult BecomeSeller()
    {
      List<Category> listcategory = new List<Category>();
      listcategory = adminService.GetListCategoryBy();

      if (listcategory != null)
      {
        List<Category> listSubCategory = new List<Category>();
        listSubCategory = adminService.GetListSubCategoryByCategoryParentId(1);

        ViewBag.subcategory = listSubCategory;
        ViewBag.listcategory = listcategory;
      }
      return View();
    }
    [HttpGet("/EditProfile")]
    public IActionResult EditProfile()
    {
      var userId = HttpContext.Session.GetInt32("UserId");
      var user = userService.GetUsersByID(userId);
      if (user != null)
      {
        ViewBag.UserId = userId;
        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;
        ViewBag.FullName = user.FullName;
        ViewBag.Country = user.Country;
        ViewBag.Address = user.Address;

      }
      return View();

    }



  }
}