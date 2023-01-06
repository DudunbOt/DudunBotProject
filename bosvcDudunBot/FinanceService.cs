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
    public class FinanceService : IFinanceService
    {

        private readonly IFinanceDao _financeDao;
        private readonly List<string> _errors;

        public FinanceService()
        {
            _financeDao = new InSQLFInanceDao();
            _errors = new List<string>();
        }

        public Finance Create(Finance newData)
        {
            return _financeDao.Create(newData);
        }

        public bool Delete(int id)
        {
            return _financeDao.Delete(id);
        }

        public List<Finance> FilterSearch(string searchString, string sortOrder)
        {
            return _financeDao.FilterSearch(searchString, sortOrder);
        }

        public List<Finance> GetAll()
        {
            return _financeDao.GetAll();
        }

        public Finance GetById(int id)
        {
            return _financeDao.GetById(id);
        }

        public int GetCount()
        {
            return _financeDao.GetCount();
        }

        public decimal GetCurrentBalance()
        {
            return _financeDao.GetCurrentBalance();
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public List<string> GetOutcomePerMonth()
        {
            return _financeDao.GetOutcomePerMonth();
        }

        public decimal GetSum(string type)
        {
            return _financeDao.GetSum(type);
        }

        public string GetYearlyOutcome()
        {
            return _financeDao.GetYearlyOutcome();
        }

        public bool Update(Finance data, int id)
        {
            return _financeDao.Update(data, id);
        }

    }
}
