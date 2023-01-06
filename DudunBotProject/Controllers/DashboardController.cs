﻿using bosvcDudunBot;
using mdlDudunBot;
using svcDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DudunBotProject.Controllers
{

    public class DashboardController : Controller
    {

        private readonly IFinanceService _financeService;
        private readonly IUserService _userService;

        public DashboardController()
        {
            _financeService = new FinanceService();
            _userService = new UserService();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login");

            ViewBag.Title = "Dashboard";
            ViewBag.CountFinance = _financeService.GetCount();
            ViewData["Finance"] = _financeService.GetAll();
            ViewData["OutcomePerMonth"] = _financeService.GetOutcomePerMonth();
            ViewBag.Yearly = _financeService.GetYearlyOutcome();

            ViewBag.CurrentBalance = _financeService.GetCurrentBalance();
            ViewBag.Saving = _financeService.GetSum("Saving");
            ViewBag.Rent_Out = _financeService.GetSum("Rent_Out");
            ViewBag.Rent_In = _financeService.GetSum("Rent_In");

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(FormCollection param)
        {
            string username = param["Username"];
            string password = param["Password"];

            if (_userService.Auth(username, password) == false)
                return View();

            Session["User"] = username;
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

    }
}