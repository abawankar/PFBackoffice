using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class Country : ICountry
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Nationality { get; set; }
    }
}
