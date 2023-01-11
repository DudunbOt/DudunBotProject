using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace svcDudunBot
{
    public interface IBlogService
    {
        Blog Create(Blog newPortfolio);
        List<Blog> GetAll();
        bool Update(Blog portfolio, int id);
        bool Delete(int id);
        Blog GetById(int id);
        int GetCount();
        List<string> GetErrors();
        List<Blog> FilterSearch(string searchString, string sortOrder);
    }
}
