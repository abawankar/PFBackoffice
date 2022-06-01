using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.CMC
{
    public class CMCTransactionModel: Domain.Implementation.Transaction.CMCTransaction
    {
        public int MemberId { get; set; }
        public string Month { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public double TotalPayable { get { return Honorarium + Contingency + TAOPE; } }
        public double NetPayable { get { return TotalPayable+Review + Meeting + Claim; } }
        public double OtherExpence { get { return Contingency+TAOPE + Review + Meeting + Claim; } }
    }

    public class CMCTransactionRepository : RepositoryModel<CMCTransactionModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CMCTransactionModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<CMCTransactionModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public override CMCTransactionModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(CMCTransactionModel obj)
        {
            throw new NotImplementedException();
        }
    }
}