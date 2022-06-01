using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ICompany
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string StateCode { get; set; }
        string PAN { get; set; }
        string ServiceTax { get; set; }
        string Emailid { get; set; }
        string PhoneNo { get; set; }
        string CIN { get; set; }
        string GST { get; set; }
        DateTime? RegDate { get; set; }
        string RegNumber { get; set; }
        string WebSite { get; set; }
        string EstablishmentCode { get; set; }
        string ESIRegistrationNumber { get; set; }
        IList<IDigitalSignature> DigitalSign { get; set; }
        IStatutoryCode StatutoryCode { get; set; }

    }
}
