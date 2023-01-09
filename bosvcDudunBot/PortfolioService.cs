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
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioDao _portfolioDao;
        private readonly List<string> _errors;

        public PortfolioService()
        {
            _portfolioDao = new InSQLPortfolioDao();
            _errors = new List<string>();
        }

        public Portfolio Create(Portfolio newPortfolio)
        {
            return _portfolioDao.Create(newPortfolio);
        }

        public bool Delete(int id)
        {
            return _portfolioDao.Delete(id);
        }

        public List<Portfolio> FilterSearch(string searchString, string sortOrder)
        {
            var listOfData = _portfolioDao.GetAll();
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
                case "title":
                    data = data.OrderBy(s => s.Title);
                    break;
            }

            return data.ToList();
        }

        public List<Portfolio> GetAll()
        {
            return _portfolioDao.GetAll();
        }

        public Portfolio GetById(int id)
        {
            return _portfolioDao.GetById(id);
        }

        public int GetCount()
        {
            return _portfolioDao.GetCount();
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public bool Update(Portfolio portfolio, int id)
        {
            return _portfolioDao.Update(portfolio, id);
        }
    }
}
