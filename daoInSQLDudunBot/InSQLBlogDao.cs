using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daoInSQLDudunBot
{
    public class InSQLBlogDao : IBlogDao
    {
        readonly DudunBotEntities Db = new DudunBotEntities();
        public Blog Create(Blog newBlog)
        {
            Db.Blogs.Add(newBlog);
            Db.SaveChanges();

            return newBlog;
        }

        public bool Delete(int id)
        {
            var data = GetById(id);
            if (data == null)
                return false;

            Db.Blogs.Remove(data);
            Db.SaveChanges();

            return true;
        }

        public bool Update(Blog blog, int id)
        {
            var data = GetById(id);
            if (data == null)
                return false;

            data.Author = blog.Author;
            data.Title = blog.Title;
            data.Subtitle = blog.Subtitle;
            data.Content = blog.Content;
            data.Image = blog.Image;
            data.UpdatedAt = DateTime.Now.ToShortDateString();

            Db.Entry(data).State = EntityState.Modified;
            Db.SaveChanges();

            return true;
        }

        public List<Blog> GetAll()
        {
            return Db.Blogs.ToList();
        }

        public Blog GetById(int id)
        {
            if (!Db.Blogs.Any(e => e.Id == id))
                return null;

            return Db.Blogs.FirstOrDefault(e => e.Id == id);
        }

        public int GetCount()
        {
            return Db.Blogs.Count();
        }
    }
}
