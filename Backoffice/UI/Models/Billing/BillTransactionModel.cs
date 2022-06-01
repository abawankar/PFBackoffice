using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Billing
{
    public class ServicePeriodModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class BillTransactionModel: Domain.Implementation.Transaction.BillTransaction
    {
        public int ServiceId { get; set; }

    }

    public class BillTransactionRepository : RepositoryModel<BillTransactionModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(BillTransactionModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<BillTransactionModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public override BillTransactionModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(BillTransactionModel obj)
        {
            throw new NotImplementedException();
        }
    }
}