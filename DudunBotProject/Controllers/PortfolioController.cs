using bosvcDudunBot;
using mdlDudunBot;
using PagedList;
using svcDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DudunBotProject.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public Portfolio BindData(Portfolio data, FormCollection param)
        {
            data.Title = param["Title"];
            data.Image = param["Image"];
            data.Type = param["Type"];

            return data;
        }

        public Portfolio DataPreview(int id)
        {
            if (Session["User"] == null)
                return null;

            var data = _portfolioService.GetById(id);

            return data;
        }

        public PortfolioController()
        {
            _portfolioService = new PortfolioService();
        }
        // GET: Portfolio
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "title" ? "title_desc" : "title";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var data = _portfolioService.FilterSearch(searchString, sortOrder);

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection param)
        {
            var data = new Portfolio();
            BindData(data, param);

            if (_portfolioService.Create(data) == null)
                return View(data);

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var data = DataPreview(id);

            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        public ActionResult Edit(int id)
        {
            var data = DataPreview(id);

            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection param, int id)
        {
            var data = _portfolioService.GetById(id);
            if (data == null)
                return RedirectToAction("Index");

            BindData(data, param);
            if (_portfolioService.Update(data, id) == false)
            {
                var err = _portfolioService.GetErrors();
                foreach (var item in err)
                    ModelState.AddModelError(Guid.NewGuid().ToString(), item);

                return View(data);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var data = DataPreview(id);

            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var data = DataPreview(id);
            if (data == null)
                return RedirectToAction("Index");
            if (_portfolioService.Delete(id) == false)
            {
                var err = _portfolioService.GetErrors();
                foreach (var item in err)
                    ModelState.AddModelError(Guid.NewGuid().ToString(), item);

                return View(data);
            }

            return RedirectToAction("Index");
        }
    }
}