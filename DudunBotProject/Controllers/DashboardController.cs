using bosvcDudunBot;
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

        public DashboardController()
        {
            _financeService = new FinanceService();
        }


        // GET: Dashboard
        public ActionResult Index()
        {
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
    }
}