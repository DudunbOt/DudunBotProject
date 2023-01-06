using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdlDudunBot
{
    public interface IFinanceDao
    {
        Finance Create(Finance newData);
        List<Finance> GetAll();
        Finance GetById(int id);
        bool Update(Finance data, int id);
        bool Delete(int id);
        int GetCount();
        List<Finance> FilterSearch(string searchString, string sortOrder);
        decimal GetSum(string type);
        decimal GetCurrentBalance();
        List<string> GetOutcomePerMonth();
        string GetYearlyOutcome();

    }
}
