using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class LoginInfo : ILoginInfo
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
