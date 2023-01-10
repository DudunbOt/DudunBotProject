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
            var listOfData = _financeDao.GetAll();
            var data = from s in listOfData select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.Source.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "source_desc":
                    data = data.OrderByDescending(s => s.Source);
                    break;
                case "Source":
                    data = data.OrderBy(s => s.Source);
                    break;
                case "Date":
                    data = data.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.Date);
                    break;
                default:
                    data = data.OrderByDescending(s => s.Date);
                    break;
            }

            return data.ToList();
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
            var currentBalance = GetSum("Income") + GetSum("Rent_In") - GetSum("Outcome") - GetSum("Rent_Out");

            return currentBalance;
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public List<string> GetOutcomePerMonth()
        {
            var finances = GetAll();
            var outcome = finances
            .Select(k => new { k.Date.Day, k.Date.Month, k.Date.Year, k.Nominal, k.Type })
            .Where(k => k.Month == DateTime.Now.Month && k.Year == DateTime.Now.Year && k.Type == "Outcome")
            .GroupBy(x => new { x.Day, x.Year, x.Month }, (key, group) => new
            {
                dy = key.Day,
                yr = key.Year,
                mnth = key.Month,
                OutcomePerMonth = group.Sum(k => k.Nominal)
            })
            .ToList();

            var day = new List<int>();
            var uang = new List<int>();
            var data = new List<string>();

            foreach (var item in outcome)
            {
                day.Add(item.dy);
                uang.Add(item.OutcomePerMonth);
            }

            var labels = string.Join("|", day);
            var values = string.Join("|", uang);
            data.Add(labels);
            data.Add(values);


            return data;
        }

        public decimal GetSum(string type)
        {
            var finances = GetAll();
            switch (type)
            {
                case "Income":
                    return (from x in finances where x.Type == "Income" select x.Nominal).Sum();
                case "Outcome":
                    return (from x in finances where x.Type == "Outcome" select x.Nominal).Sum();
                case "Saving":
                    return (from x in finances where x.Type == "Saving" select x.Nominal).Sum();
                case "Rent_In":
                    return (from x in finances where x.Type == "Rent_In" select x.Nominal).Sum();
                case "Rent_Out":
                    return (from x in finances where x.Type == "Rent_Out" select x.Nominal).Sum();
            }
            return 0;
        }

        public string GetYearlyOutcome()
        {
            var outcome = GetAll()
            .Select(k => new { k.Date.Month, k.Date.Year, k.Nominal, k.Type })
            .Where(k => k.Type == "Outcome")
            .GroupBy(x => new { x.Year, x.Month }, (key, group) => new
            {
                yr = key.Year,
                mnth = key.Month,
                monthlyOutcome = group.Sum(k => k.Nominal)
            }).ToList(); ;

            var nominal = new List<int>();

            foreach (var item in outcome)
            {
                nominal.Add(item.monthlyOutcome);
            }

            string monthlyOutcomedata = string.Join("|", nominal);

            return monthlyOutcomedata;
        }

        public bool Update(Finance data, int id)
        {
            return _financeDao.Update(data, id);
        }

    }
}
