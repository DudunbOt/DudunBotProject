using bosvcDudunBot;
using svcDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DudunBotProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IBlogService _blogService;

        public HomeController()
        {
            _portfolioService = new PortfolioService();
            _blogService = new BlogService();
        }
        public ActionResult Index()
        {
            ViewData["Portfolio"] = _portfolioService.GetAll();
            ViewData["Blog"] = _blogService.GetAll().Take(4).OrderByDescending(x => x.UpdatedAt);
            return View();
        }
    }
}