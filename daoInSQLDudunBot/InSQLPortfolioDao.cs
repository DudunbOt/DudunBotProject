using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daoInSQLDudunBot
{
    public class InSQLPortfolioDao : IPortfolioDao
    {
        readonly DudunBotEntities Db = new DudunBotEntities();
        public Portfolio Create(Portfolio newPortfolio)
        {
            Db.Portfolios.Add(newPortfolio);
            Db.SaveChanges();

            return newPortfolio;
        }

        public bool Delete(int id)
        {
            var data = GetById(id);
            if (data == null)
                return false;

            Db.Portfolios.Remove(data);
            Db.SaveChanges();

            return true;
        }

        public List<Portfolio> GetAll()
        {
            return Db.Portfolios.ToList();
        }

        public Portfolio GetById(int id)
        {
            if (!Db.Finances.Any(e => e.Id == id))
                return null;

            return Db.Portfolios.FirstOrDefault(e => e.Id == id);
        }

        public int GetCount()
        {
            return Db.Portfolios.Count();
        }

        public bool Update(Portfolio portfolio, int id)
        {
            var data = GetById(id);
            if (data == null)
                return false;

            data.Title = portfolio.Title;
            data.Image = portfolio.Image;
            data.Type = portfolio.Type;

            Db.Entry(data).State = EntityState.Modified;
            Db.SaveChanges();

            return true;
        }
    }
}
