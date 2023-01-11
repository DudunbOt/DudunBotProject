using bosvcDudunBot;
using svcDudunBot;
using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;

namespace DudunBotProject.Controllers
{
    
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController()
        {
            _blogService = new BlogService();
        }

        public Blog BindData(Blog blog, FormCollection param)
        {
            blog.Title = param["Title"];
            blog.Subtitle = param["Subtitle"];
            blog.Content = param["Content"];
            blog.Author = param["Author"];

            return blog;
        }

        public string GetImage(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength < 1)
                return null;

            string path = Path.Combine(Server.MapPath("~/Assets/Upload/Blog/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);

            return Path.GetFileName(file.FileName);
        }

        public Blog dataPreview(int id)
        {
            if (Session["User"] == null)
                return null;

            return _blogService.GetById(id);
        }

        // GET: Blog
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Dashboard");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "title" ? "title_desc" : "title";
            ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var data = _blogService.FilterSearch(searchString, sortOrder);

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
            HttpPostedFileBase file = Request.Files["Image"];
            var ImgPath = GetImage(file);
            var newBlog = new Blog()
            {
                Image = ImgPath,
                CreatedAt = DateTime.Now.ToShortDateString(),
                UpdatedAt = DateTime.Now.ToShortDateString()
            };

            BindData(newBlog, param);

            if (_blogService.Create(newBlog) == null)
                return View(newBlog);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (dataPreview(id) == null)
                return RedirectToAction("Index");

            return View(dataPreview(id));
        }

        [HttpPost]
        public ActionResult Edit(FormCollection param, int id)
        {
            HttpPostedFileBase file = Request.Files["Image"];
            var data = dataPreview(id);
            var ImgPath = GetImage(file);
            if (ImgPath != null)
                data.Image = ImgPath;

            BindData(data, param);
            if(_blogService.Update(data, id) == false)
            {
                var err = _blogService.GetErrors();
                foreach (var item in err)
                    ModelState.AddModelError(Guid.NewGuid().ToString(), item);

                return View(data);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            if (dataPreview(id) == null)
                return RedirectToAction("Index");

            return View(dataPreview(id));
        }

        public ActionResult Delete(int id)
        {
            if (dataPreview(id) == null)
                return RedirectToAction("Index");

            return View(dataPreview(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var data = dataPreview(id);
            if(_blogService.Delete(id) == false)
            {
                var err = _blogService.GetErrors();
                foreach (var item in err)
                    ModelState.AddModelError(Guid.NewGuid().ToString(), item);

                return View(data);
            }

            return RedirectToAction("Index");
        }
    }
}