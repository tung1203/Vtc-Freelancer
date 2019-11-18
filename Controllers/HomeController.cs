﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vtc_Freelancer.Models;
using Vtc_Freelancer.Services;
using Microsoft.AspNetCore.Http;

namespace Vtc_Freelancer.Controllers
{
    public class HomeController : Controller
    {
        // private MyDbContext dbContext;
        private UserService userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            this.userService = userService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                Users userads = userService.GetUsersByID(userId);
                ViewBag.UserName = userads.UserName;
                return View();
            }
            return Redirect("/Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            Users userads = userService.GetUsersByID(userId);
            ViewBag.UserName = userads.UserName;
            return View();
        }


    }
}
