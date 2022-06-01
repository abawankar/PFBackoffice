using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Transaction
{
    public class ESIMonthlyContModel:Domain.Implementation.Transaction.ESIMonthlyCont
    {
        public int CompId { get; set; }
        public string CompName { get; set; }
    }

    public class ESIMonthlyContRepository : RepositoryModel<ESIMonthlyContModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(ESIMonthlyContModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<ESIMonthlyContModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public override ESIMonthlyContModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(ESIMonthlyContModel obj)
        {
            throw new NotImplementedException();
        }
    }
}