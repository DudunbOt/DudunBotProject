using daoInSQLDudunBot;
using mdlDudunBot;
using svcDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bosvcDudunBot
{
    public class BlogService : IBlogService
    {
        private readonly IBlogDao _blogDao;
        private readonly List<string> _errors;

        public BlogService()
        {
            _blogDao = new InSQLBlogDao();
            _errors = new List<string>();
        }

        public Blog Create(Blog newBlog)
        {
            return _blogDao.Create(newBlog);
        }

        public bool Delete(int id)
        {
            return _blogDao.Delete(id);
        }

        public bool Update(Blog blog, int id)
        {
            return _blogDao.Update(blog, id);
        }

        public List<Blog> GetAll()
        {
            return _blogDao.GetAll();
        }

        public Blog GetById(int id)
        {
            return _blogDao.GetById(id);
        }

        public int GetCount()
        {
            return _blogDao.GetCount();
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public List<Blog> FilterSearch(string searchString, string sortOrder)
        {
            var listOfData = _blogDao.GetAll();
            var data = from s in listOfData select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    data = data.OrderByDescending(s => s.Title);
                    break;
                case "Title":
                    data = data.OrderBy(s => s.Title);
                    break;
                case "Date":
                    data = data.OrderBy(s => s.UpdatedAt);
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.UpdatedAt);
                    break;
                default:
                    data = data.OrderByDescending(s => s.UpdatedAt);
                    break;
            }

            return data.ToList();
        }
    }
}
