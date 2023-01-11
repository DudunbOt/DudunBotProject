using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdlDudunBot
{
    public interface IBlogDao
    {
        Blog Create(Blog newBlog);
        List<Blog> GetAll();
        Blog GetById(int id);
        bool Update(Blog blog, int id);
        bool Delete(int id);
        int GetCount();
    }
}
