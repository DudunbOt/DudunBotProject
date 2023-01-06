using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daoInSQLDudunBot
{
    public class InSQLFInanceDao : IFinanceDao
    {
        readonly DudunBotEntities Db = new DudunBotEntities();

        public Finance Create(Finance newData)
        {
            Db.Finances.Add(newData);
            Db.SaveChanges();

            return newData;
        }

        public bool Delete(int id)
        {
            if (GetById(id) == null)
                return false;

            Db.Finances.Remove(GetById(id));
            Db.SaveChanges();

            return true;
        }

        public List<Finance> FilterSearch(string searchString, string sortOrder)
        {
            var listOfData = GetAll();
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
            return Db.Finances.ToList();
        }

        public Finance GetById(int id)
        {
            if (!Db.Finances.Any(e => e.Id == id))
                return null;

            return Db.Finances.Where(e => e.Id == id).FirstOrDefault();
        }

        public int GetCount()
        {
            return Db.Finances.Count();
        }

        public decimal GetCurrentBalance()
        {
            var currentBalance = GetSum("Income") + GetSum("Rent_In") - GetSum("Outcome") - GetSum("Rent_Out");

            return currentBalance;
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
                OutcomePerMonth = group.Sum(k => k.Nominal).Value
            })
            .ToList();

            var day = new List<int>();
            var uang = new List<int>();
            var data = new List<string>();

            foreach(var item in outcome)
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
                    return (from x in finances where x.Type == "Income" select x.Nominal).Sum().Value;
                case "Outcome":
                    return (from x in finances where x.Type == "Outcome" select x.Nominal).Sum().Value;
                case "Saving":
                    return (from x in finances where x.Type == "Saving" select x.Nominal).Sum().Value;
                case "Rent_In":
                    return (from x in finances where x.Type == "Rent_In" select x.Nominal).Sum().Value;
                case "Rent_Out":
                    return (from x in finances where x.Type == "Rent_Out" select x.Nominal).Sum().Value;
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
                nominal.Add(item.monthlyOutcome.Value);
            }

            string monthlyOutcomedata = string.Join("|", nominal);

            return monthlyOutcomedata;
        }

        public bool Update(Finance data, int id)
        {
            var updatedData = GetById(id);
            if (updatedData == null)
                return false;

            updatedData.Source = data.Source;
            updatedData.Nominal = data.Nominal;
            updatedData.Date = data.Date;
            updatedData.Type = data.Type;
            

            Db.Entry(updatedData).State = EntityState.Modified;
            Db.SaveChanges();

            return true;
        }
    }
}
