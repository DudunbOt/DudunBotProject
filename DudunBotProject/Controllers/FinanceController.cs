using bosvcDudunBot;
using mdlDudunBot;
using svcDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data;

namespace DudunBotProject.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController()
        {
            _financeService = new FinanceService();
        }

        public Finance BindData(Finance data, FormCollection param)
        {
            data.Source = param["Source"];
            data.Nominal = int.Parse(param["Nominal"]);
            if(param["Date"] == null)
            {
                param["Date"] = DateTime.Today.ToString();
            }
            data.Date = DateTime.Parse(param["Date"]);
            data.Type = param["Type"];

            return data;
        }

        // GET: Finance
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "Source" ? "source_desc" : "Source";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var data = _financeService.FilterSearch(searchString, sortOrder);

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(data.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Create()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            var data = new Finance();
            return View(data);
        }

        [HttpPost]
        public ActionResult Create(FormCollection param)
        {
            var data = new Finance();
            BindData(data, param);

            if (_financeService.Create(data) == null)
            {
                return View(data);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            var data = _financeService.GetById(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        public ActionResult Edit(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            var data = _financeService.GetById(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection param, int id)
        {
            var data = _financeService.GetById(id);
            if (data == null)
                return RedirectToAction("Index");

            BindData(data, param);
            if(_financeService.Update(data, data.Id) == false)
            {
                var err = _financeService.GetErrors();
                foreach (var item in err)
                    ModelState.AddModelError(Guid.NewGuid().ToString(), item);

                return View(data);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            var data = _financeService.GetById(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var data = _financeService.GetById(id);
            if (data == null)
                return RedirectToAction("Index");

            if(_financeService.Delete(id) == false)
            {
                var err = _financeService.GetErrors();
                foreach (var item in err)
                    ModelState.AddModelError(Guid.NewGuid().ToString(), item);

                return View(data);
            }

            return RedirectToAction("Index");
        }
    }
}