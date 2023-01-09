using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace svcDudunBot
{
    public interface IPortfolioService
    {
        Portfolio Create(Portfolio newPortfolio);
        List<Portfolio> GetAll();
        bool Update(Portfolio portfolio, int id);
        bool Delete(int id);
        Portfolio GetById(int id);
        int GetCount();
        List<string> GetErrors();
        List<Portfolio> FilterSearch(string searchString, string sortOrder);
    }
}
