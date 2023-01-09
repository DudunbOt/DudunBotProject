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
