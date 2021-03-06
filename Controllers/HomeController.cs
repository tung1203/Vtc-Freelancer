﻿﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vtc_Freelancer.Models;
using Vtc_Freelancer.Services;
using Microsoft.AspNetCore.Http;
using Vtc_Freelancer.ActionFilter;
using System;
using PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vtc_Freelancer.Controllers
{
    public class HomeController : Controller
    {
        private UserService userService;
        private AdminService adminService;
        private OrderService orderService;
        private GigService gigService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(GigService gigService, UserService userService, AdminService adminService, OrderService orderService, ILogger<HomeController> logger)
        {
            this.gigService = gigService;
            this._logger = logger;
            this.orderService = orderService;
            this.userService = userService;
            this.adminService = adminService;
        }

        [HttpGet("/HomePage")]
        public IActionResult HomePage()
        {
            List<Category> listcategory = new List<Category>();
            listcategory = adminService.GetListCategoryBy();
            foreach (var item in listcategory)
            {
                item.subsCategory = adminService.GetListSubCategoryByParentId(item.CategoryId);
            }
            List<Service> services = new List<Service>();
            services = adminService.GetListServices("");
            if (listcategory != null)
            {
                ViewBag.listcategory = listcategory;
            }

            return View();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return Redirect("/HomePage");
            }
            HttpContext.Session.Remove("Quantity");
            List<Category> listcategory = new List<Category>();
            listcategory = adminService.GetListCategoryBy();
            foreach (var item in listcategory)
            {
                item.subsCategory = adminService.GetListSubCategoryByParentId(item.CategoryId);
            }
            List<Service> services = new List<Service>();
            services = adminService.GetListServices("");
            foreach (var item in services)
            {
                item.ListImage = adminService.GetListImageService(item.ServiceId);
                List<Package> ListPackage = gigService.GetPackageByServiceID(item.ServiceId);
                item.listPackage = ListPackage;
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                Users userads = userService.GetUserByUserId(userId);
                ViewBag.UserName = userads.UserName;
                ViewBag.userAvatar = userads.Avatar;
                ViewBag.ListOrder = orderService.GetListOrderbyUserId(userId);

                ViewBag.IsSeller = HttpContext.Session.GetInt32("IsSeller");
                // HttpContext.Session.Remove("IsSeller");
                ViewBag.SellerId = HttpContext.Session.GetInt32("SellerId");
            }
            else
            {
                return Redirect("/HomePage");
            }

            if (listcategory != null)
            {
                ViewBag.listcategory = listcategory;

            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(services);
        }
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            Users userads = userService.GetUserByUserId(userId);
            ViewBag.UserName = userads.UserName;
            return View();
        }
        public IActionResult Search(string search_content)
        {
            List<Category> listcategory = new List<Category>();
            listcategory = adminService.GetListCategoryBy();
            foreach (var item in listcategory)
            {
                item.subsCategory = adminService.GetListSubCategoryByParentId(item.CategoryId);
            }
            if (listcategory != null)
            {
                ViewBag.listcategory = listcategory;
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            if (search_content == null)
            {
                return Redirect("/");
            }
            List<Service> services = new List<Service>();
            services = adminService.GetListServices(search_content);
            foreach (var item in services)
            {
                item.ListImage = adminService.GetListImageService(item.ServiceId);
                List<Package> ListPackage = gigService.GetPackageByServiceID(item.ServiceId);
                item.listPackage = ListPackage;
            }
            ViewBag.ListServicesSearch = services;
            // if (page == null) page = 1;

            // int pageSize = 30;

            // // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            // int pageNumber = (page ?? 1);
            ViewBag.searchResult = search_content;

            return View();
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
